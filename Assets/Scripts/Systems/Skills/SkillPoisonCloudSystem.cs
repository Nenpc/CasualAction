using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct SkillPoisonCloudSystem : ISystem
{
    private float _nextCastTime;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CharacterPoisonCloudSkillComponent>();
        state.RequireForUpdate<CharacterTag>();
        state.RequireForUpdate<GameplayTimeComponent>();
        state.RequireForUpdate<GameStateComponent>();
        
        _nextCastTime = 0f;
    }

    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameStateComponent>();
        if (!gameState.IsGameStarted || gameState.IsPaused || gameState.IsGameOver) 
            return;

        var gameplayTime = SystemAPI.GetSingleton<GameplayTimeComponent>();
        var skill = SystemAPI.GetSingleton<CharacterPoisonCloudSkillComponent>();

        if (!skill.IsEnabled) 
            return;

        if (gameplayTime.ElapsedTime < _nextCastTime) 
            return;

            ApplyPoisonCloudDamage(ref state, in skill);

        // Спавним визуальный префаб скила в позиции игрока
        if (skill.Prefab != Entity.Null)
        {
            var characterEntity = SystemAPI.GetSingletonEntity<CharacterTag>();
            var characterTransform = SystemAPI.GetComponentRO<LocalTransform>(characterEntity);
            var spawnedEntity = state.EntityManager.Instantiate(skill.Prefab);
            state.EntityManager.SetComponentData(spawnedEntity, characterTransform.ValueRO);
        }

        _nextCastTime = (float)gameplayTime.ElapsedTime + skill.Cooldown;
    }

    [BurstCompile]
    private void ApplyPoisonCloudDamage(ref SystemState state, in CharacterPoisonCloudSkillComponent skill)
    {
        var characterEntity = SystemAPI.GetSingletonEntity<CharacterTag>();
        var characterTransform = SystemAPI.GetComponentRO<LocalTransform>(characterEntity);

        float3 center = characterTransform.ValueRO.Position;

        foreach (var (enemyTransform, health) in 
                 SystemAPI.Query<RefRO<LocalTransform>, RefRW<HealthComponent>>()
                          .WithAll<EnemyTag>())
        {
            if (health.ValueRO.CurrentHealth <= 0)
                continue;

            float distanceSq = math.distancesq(center, enemyTransform.ValueRO.Position);

            if (distanceSq <= skill.Radius * skill.Radius)
            {
                health.ValueRW.CurrentHealth -= skill.Damage;
            }
        }
    }
}
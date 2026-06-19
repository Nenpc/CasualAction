using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct SkillConeAttackSystem : ISystem
{
    private float _nextCastTime;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CharacterConeAttackSkillComponent>();
        //state.RequireForUpdate<CharacterTag>();
        //state.RequireForUpdate<GameStateComponent>();
        //state.RequireForUpdate<GameplayTimeComponent>();
        
        _nextCastTime = 0f;
    }

    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameStateComponent>();
        if (!gameState.IsGameStarted || gameState.IsPaused || gameState.IsGameOver) 
            return;

        var gameplayTime = SystemAPI.GetSingleton<GameplayTimeComponent>();
        var skill = SystemAPI.GetSingleton<CharacterConeAttackSkillComponent>();

        if (!skill.IsEnabled)
            return;

        if (gameplayTime.ElapsedTime < _nextCastTime) 
            return;

        TryPerformConeAttack(ref state, in skill);

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
    private void TryPerformConeAttack(ref SystemState state, in CharacterConeAttackSkillComponent skill)
    {
        var characterEntity = SystemAPI.GetSingletonEntity<CharacterTag>();
        var characterTransform = SystemAPI.GetComponentRO<LocalTransform>(characterEntity);
        var characterRotation = characterTransform.ValueRO.Rotation;

        float3 forward = math.mul(characterRotation, new float3(0, 0, 1));

        foreach (var (enemyTransform, health, enemyEntity) in 
                SystemAPI.Query<RefRO<LocalTransform>, RefRW<HealthComponent>>().WithAll<EnemyTag>().WithEntityAccess())
        {
            // Пропускаем уже мёртвых врагов
            if (health.ValueRO.CurrentHealth <= 0)
                continue;

            float3 toEnemy = enemyTransform.ValueRO.Position - characterTransform.ValueRO.Position;
            float distanceSq = math.lengthsq(toEnemy);

            if (distanceSq > skill.Radius * skill.Radius || distanceSq < 0.0001f)
                continue;

            float3 dirToEnemy = math.normalize(toEnemy);

            float angle = math.degrees(math.acos(math.dot(forward, dirToEnemy)));

            if (angle <= skill.Angle * 0.5f)
            {
                health.ValueRW.CurrentHealth -= skill.Damage;
            }
        }
    }
}
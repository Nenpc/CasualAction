using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct PlayerPoisonCloudSkillSystem : ISystem
{
    private float _nextCastTime;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerPoisonCloudSkillComponent>();
        state.RequireForUpdate<PlayerTag>();
        state.RequireForUpdate<GameplayTimeComponent>();
        state.RequireForUpdate<GameStateComponent>();
        
        _nextCastTime = 0f;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameStateComponent>();
        if (!gameState.IsGameStarted || gameState.IsPaused || gameState.IsGameOver) 
            return;

        var gameplayTime = SystemAPI.GetSingleton<GameplayTimeComponent>();
        var skill = SystemAPI.GetSingleton<PlayerPoisonCloudSkillComponent>();

        if (!skill.IsEnabled) 
            return;

        if (gameplayTime.ElapsedTime < _nextCastTime) 
            return;

        ApplyPoisonCloudDamage(ref state, in skill);

        _nextCastTime = (float)gameplayTime.ElapsedTime + skill.Cooldown;
    }

    [BurstCompile]
    private void ApplyPoisonCloudDamage(ref SystemState state, in PlayerPoisonCloudSkillComponent skill)
    {
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        var playerTransform = SystemAPI.GetComponentRO<LocalTransform>(playerEntity);

        float3 center = playerTransform.ValueRO.Position;

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
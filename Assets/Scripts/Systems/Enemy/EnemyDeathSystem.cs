using Unity.Burst;
using Unity.Entities;

[BurstCompile]
[UpdateBefore(typeof(EnemyCleanupSystem))]
public partial struct EnemyDeathSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameStateComponent>();
        if (!gameState.IsGameStarted || gameState.IsGameOver)
            return;

        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        
        foreach (var (health, entity) in
                 SystemAPI.Query<RefRO<HealthComponent>>()
                     .WithAll<EnemyTag>()
                     .WithNone<DeadTag>()
                     .WithEntityAccess())
        {
            if (health.ValueRO.CurrentHealth <= 0f)
            {
                ecb.AddComponent<DeadTag>(entity);
            }
        }
    }
}
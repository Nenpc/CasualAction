using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct EnemyMoveSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerTag>();
        state.RequireForUpdate<GameplayTimeComponent>();
        state.RequireForUpdate<GameStateComponent>();
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameStateComponent>();
        if (!gameState.IsGameStarted || gameState.IsPaused || gameState.IsGameOver)
            return;

        var gameplayTime = SystemAPI.GetSingleton<GameplayTimeComponent>();

        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        float3 playerPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;

        foreach (var (transform, speed) in 
                SystemAPI.Query<RefRW<LocalTransform>, RefRO<MovementSpeedComponent>>()
                        .WithAll<EnemyTag>())
        {
            float3 enemyPos = transform.ValueRO.Position;
            float3 direction = playerPosition - enemyPos;
            float distance = math.length(direction);

            if (distance <= float.Epsilon)
                continue;

            direction = math.normalize(direction);
                
            transform.ValueRW.Position += direction * speed.ValueRO.Value * gameplayTime.DeltaTime;
        }
    }
}
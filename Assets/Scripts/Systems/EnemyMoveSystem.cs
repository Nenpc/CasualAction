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
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameState>();

        if (gameState.IsGameOver)
            return;

        var deltaTime = SystemAPI.Time.DeltaTime;

        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        float3 playerPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;
        
        foreach (var (transform, speed, entity) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRW<SpeedComponent>>()
                     .WithAll<SpeedComponent>()
                     .WithNone<PlayerTag, DeadTag>()
                     .WithEntityAccess())
        {
            float3 enemyPos = transform.ValueRO.Position;
            float3 direction = playerPosition - enemyPos;
            float distance = math.length(direction);

            if (distance <= float.Epsilon)
                continue;

            direction = math.normalize(direction);
            
            float3 newPosition = enemyPos + direction * speed.ValueRO.Speed * deltaTime;

            transform.ValueRW.Position = newPosition;
        }
    }
}
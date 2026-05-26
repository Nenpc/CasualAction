using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Burst;

[BurstCompile]
public partial struct PlayerMoveSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameplayTimeComponent>();
        state.RequireForUpdate<GameStateComponent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameStateComponent>();
        if (gameState.IsPaused || gameState.IsGameOver)
            return;

        var gameplayTime = SystemAPI.GetSingleton<GameplayTimeComponent>();

        foreach (var (transform, speed, input) in 
                SystemAPI.Query<RefRW<LocalTransform>, RefRO<MovementSpeedComponent>, RefRO<PlayerInputComponent>>()
                        .WithAll<PlayerTag>())
        {
            float3 moveDir = new float3(input.ValueRO.Move.x, 0, input.ValueRO.Move.y);
            if (math.lengthsq(moveDir) > 0.001f)
            {
                moveDir = math.normalize(moveDir);
                transform.ValueRW.Position += moveDir * speed.ValueRO.Value * gameplayTime.DeltaTime;
            }
        }
    }
}
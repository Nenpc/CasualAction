using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Burst;

[BurstCompile]
public partial struct CharacterMoveSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameplayTimeComponent>();
        state.RequireForUpdate<GameStateComponent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameStateComponent>();
        if (!gameState.IsGameStarted || gameState.IsPaused || gameState.IsGameOver)
            return;
    
        var gameplayTime = SystemAPI.GetSingleton<GameplayTimeComponent>();

        foreach (var (transform, speed, input, direction) in 
                SystemAPI.Query<RefRW<LocalTransform>, RefRO<MovementSpeedComponent>, RefRO<PlayerInputComponent>, RefRW<CharacterDirectionComponent>>()
                        .WithAll<CharacterTag>())
        {
            float3 moveDir = new float3(input.ValueRO.Move.x, input.ValueRO.Move.y, 0);
            if (math.lengthsq(moveDir) > 0.001f)
            {
                moveDir = math.normalize(moveDir);
                transform.ValueRW.Position += moveDir * speed.ValueRO.Value * gameplayTime.DeltaTime;

                direction.ValueRW.Direction = new float2(moveDir.x, moveDir.y);
                if (math.lengthsq(direction.ValueRW.Direction) > 0.0001f)
                {
                    float angle = math.atan2(direction.ValueRW.Direction.y, direction.ValueRW.Direction.x);
                    transform.ValueRW.Rotation = quaternion.EulerXYZ(0, 0, angle);
                }
            }
        }
    }
}
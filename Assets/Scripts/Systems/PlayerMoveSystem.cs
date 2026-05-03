using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
public partial struct PlayerMoveSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        // Инициализация системы
    }

    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameState>();

        if (gameState.IsGameOver)
            return;

        var movement = new float3(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"),
            0
        );
        
        if (math.length(movement) <= float.Epsilon)
            return;

        foreach (var playerTransform in 
                 SystemAPI.Query<RefRW<LocalTransform>>()
                     .WithAll<PlayerTag>())
        {
            movement = math.normalize(movement);
            movement *= SystemAPI.Time.DeltaTime;
            playerTransform.ValueRW.Position += movement;
        }
    }
}
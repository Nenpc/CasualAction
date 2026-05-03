using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct CameraMoveSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameState>();

        if (gameState.IsGameOver)
            return;

        if (SystemAPI.GetSingleton<GameState>().IsGameOver) return;

        foreach (var playerTransform in
                 SystemAPI.Query<RefRW<LocalTransform>>()
                     .WithAll<PlayerTag>())
        {
            var cameraTransform = Camera.main.transform;
            cameraTransform.position = playerTransform.ValueRO.Position;
            cameraTransform.position -= 10.0f * (Vector3)playerTransform.ValueRO.Forward();  // move the camera back from the player
        }
    }
}

using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct CameraMoveSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameStateComponent>();

        if (gameState.IsGameOver)
            return;

        if (SystemAPI.GetSingleton<GameStateComponent>().IsGameOver) return;

        foreach (var characterTransform in
                 SystemAPI.Query<RefRW<LocalTransform>>()
                     .WithAll<CharacterTag>())
        {
            var cameraTransform = Camera.main.transform;
            cameraTransform.position = characterTransform.ValueRO.Position;
            cameraTransform.position -= 10.0f * (Vector3)characterTransform.ValueRO.Forward();  // move the camera back from the character
        }
    }
}

using Unity.Entities;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
public partial struct GameOverSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        var entityFactory = state.EntityManager.CreateEntity(typeof(GameState));

        state.EntityManager.SetComponentData(entityFactory, new GameState { IsGameOver = false });
    }

    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameState>();

        if (gameState.IsGameOver)
            return;

        foreach (var playerHealth in SystemAPI.Query<RefRW<Health>>()
                     .WithAll<PlayerTag>())
        {
            if (playerHealth.ValueRO.CurrentHealth <= 0)
                gameState.IsGameOver = true;
        }
    }
}


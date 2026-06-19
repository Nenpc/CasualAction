using Unity.Entities;
using Unity.Burst;

[BurstCompile]
public partial struct GameOverSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        var entityFactory = state.EntityManager.CreateEntity(typeof(GameStateComponent));

        state.EntityManager.SetComponentData(entityFactory, new GameStateComponent 
        { 
            IsGameStarted = false,
            IsGameOver = false 
        });
    }

    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameStateComponent>();

        if (gameState.IsGameOver)
            return;

        foreach (var characterHealth in SystemAPI.Query<RefRW<HealthComponent>>()
                     .WithAll<CharacterTag>())
        {
            if (characterHealth.ValueRO.CurrentHealth <= 0)
                gameState.IsGameOver = true;
        }
    }
}


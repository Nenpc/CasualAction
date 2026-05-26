using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup), OrderFirst = true)]
public partial struct GameplayTimeSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingletonRW<GameStateComponent>();
        var gameplayTime = SystemAPI.GetSingletonRW<GameplayTimeComponent>();

        if (gameState.ValueRO.IsPaused || gameState.ValueRO.IsGameOver)
        {
            gameplayTime.ValueRW.DeltaTime = 0f;
            return;
        }

        gameplayTime.ValueRW.DeltaTime = SystemAPI.Time.DeltaTime;
        gameplayTime.ValueRW.ElapsedTime += SystemAPI.Time.DeltaTime;
    }
}
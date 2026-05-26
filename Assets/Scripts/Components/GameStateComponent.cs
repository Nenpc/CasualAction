using Unity.Entities;

public struct GameStateComponent : IComponentData
{
    public bool IsGameStarted;
    public bool IsGameOver;
    public bool IsPaused;
}
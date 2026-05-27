using Unity.Entities;

public struct GameStateComponent : IComponentData
{
    public bool IsGameOver;
    public bool IsPaused;
    public bool IsGameStarted;
}
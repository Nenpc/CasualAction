using Unity.Entities;

public struct LevelComponent : IComponentData
{
    public int EnemyCount;
    public float LevelDuration;
    public int LevelId;
}
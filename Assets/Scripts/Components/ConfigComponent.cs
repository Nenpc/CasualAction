using Unity.Entities;

public struct ConfigComponent : IComponentData
{
    public Entity PlayerPrefab;
    public Entity EnemyPrefab;
    public int EnemyCount;
}
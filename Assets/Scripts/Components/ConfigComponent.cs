using Unity.Entities;

public struct ConfigComponent : IComponentData
{
    public Entity PlayerPrefab;
    public int EnemyCount;
}
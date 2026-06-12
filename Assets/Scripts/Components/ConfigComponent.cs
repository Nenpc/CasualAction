using Unity.Entities;

public struct ConfigComponent : IComponentData
{
    public Entity PlayerPrefab;
    public int EnemyCount;
}

public struct EnemyPrefabBuffer : IBufferElementData
{
    public Entity Value;
}
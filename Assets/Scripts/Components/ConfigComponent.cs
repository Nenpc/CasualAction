using Unity.Entities;

public struct ConfigComponent : IComponentData
{
    public Entity PlayerPrefab;
    public int EnemyCount;
    public Entity Enemy_1;
    public Entity Enemy_2;
    public Entity Enemy_3;
}
using Unity.Collections;
using Unity.Entities;

public struct EnemyConfigComponent : IComponentData
{
    public BlobAssetReference<EnemyConfigBlob> Config;
}

public struct EnemyConfigBlob
{
    public BlobArray<EnemyConfigBlobEntry> Enemies;
}

public struct EnemyConfigBlobEntry
{
    public Entity Prefab;
    public float Health;
    public float MoveSpeed;
    public int ExperienceReward;
}

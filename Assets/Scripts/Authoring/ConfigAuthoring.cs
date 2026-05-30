using Unity.Entities;
using Unity.Collections;
using UnityEngine;

public class ConfigAuthoring : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public EnemyConfig EnemyConfig;
    public int EnemyCount;

    class Baker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.None);
            AddComponent(entity, new ConfigComponent
            {
                PlayerPrefab = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Dynamic),
                EnemyCount = authoring.EnemyCount,
            });

            if (authoring.EnemyConfig != null && authoring.EnemyConfig.Enemies != null && authoring.EnemyConfig.Enemies.Length > 0)
            {
                var blobBuilder = new BlobBuilder(Allocator.Temp);
                ref var configBlob = ref blobBuilder.ConstructRoot<EnemyConfigBlob>();
                var blobArray = blobBuilder.Allocate(ref configBlob.Enemies, authoring.EnemyConfig.Enemies.Length);

                for (int i = 0; i < authoring.EnemyConfig.Enemies.Length; i++)
                {
                    var enemyEntry = authoring.EnemyConfig.Enemies[i];
                    blobArray[i] = new EnemyConfigBlobEntry
                    {
                        Prefab = enemyEntry != null && enemyEntry.Prefab != null
                            ? GetEntity(enemyEntry.Prefab, TransformUsageFlags.Dynamic)
                            : Entity.Null,
                        Health = enemyEntry != null ? enemyEntry.Health : 0f,
                        MoveSpeed = enemyEntry != null ? enemyEntry.MoveSpeed : 0f,
                        ExperienceReward = enemyEntry != null ? enemyEntry.ExperienceReward : 0,
                    };
                }

                AddComponent(entity, new EnemyConfigComponent
                {
                    Config = blobBuilder.CreateBlobAssetReference<EnemyConfigBlob>(Allocator.Persistent)
                });
                blobBuilder.Dispose();
            }
        }
    }
}
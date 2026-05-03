using Unity.Entities;
using UnityEngine;

public class ConfigAuthoring : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;
    public int EnemyCount;

    class Baker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.None);
            AddComponent(entity, new ConfigComponent
            {
                PlayerPrefab = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Dynamic),
                EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
                EnemyCount = authoring.EnemyCount,
            });
        }
    }
}
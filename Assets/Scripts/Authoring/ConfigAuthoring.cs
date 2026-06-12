using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;

public class ConfigAuthoring : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public EnemyConfig Config;
    public int EnemyCount;

    class Baker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.None);
            AddComponent(entity, new ConfigComponent
            {
                PlayerPrefab = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Dynamic),
                Enemy_1 = GetEntity(authoring.Config.Enemies[0].Prefab, TransformUsageFlags.Dynamic),
                Enemy_2 = GetEntity(authoring.Config.Enemies[1].Prefab, TransformUsageFlags.Dynamic),
                Enemy_3 = GetEntity(authoring.Config.Enemies[2].Prefab, TransformUsageFlags.Dynamic),
                EnemyCount = authoring.EnemyCount,
            });
        }
    }
}
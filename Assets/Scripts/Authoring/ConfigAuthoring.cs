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
                EnemyCount = authoring.EnemyCount,
            });

            var buffer = AddBuffer<EnemyPrefabBuffer>(entity);
            foreach (var enemy in authoring.Config.Enemies)
            {
                buffer.Add(new EnemyPrefabBuffer 
                { 
                    Value = GetEntity(enemy.Prefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}
using Unity.Entities;
using UnityEngine;

public class HealthAuthoring : MonoBehaviour
{
    public float MaxHealth;

    class Baker : Baker<HealthAuthoring>
    {
        public override void Bake(HealthAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.None);
            AddComponent(entity, new Health
            {
                MaxHealth = authoring.MaxHealth,
                CurrentHealth = authoring.MaxHealth
            });
        }
    }
}
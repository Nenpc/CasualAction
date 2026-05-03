using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace Authoring
{
    public class URPMateiralPropertyBaseColorAuthoring : MonoBehaviour
    {
        class Baker : Baker<URPMateiralPropertyBaseColorAuthoring>
        {
            public override void Bake(URPMateiralPropertyBaseColorAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent(entity, new URPMaterialPropertyBaseColor
                {
                    Value = new float4(1,1,1,1)
                });
            }
        }
    }
}
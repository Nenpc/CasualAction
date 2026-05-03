using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

public class CharacterAuthoring : MonoBehaviour
{
    public float Speed;
    
    class Baker : Baker<CharacterAuthoring>
    {
        public override void Bake(CharacterAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.WorldSpace);
            
            AddComponent(entity, new SpeedComponent
                {
                    Speed = authoring.Speed
                }
            );
        }
    }
}
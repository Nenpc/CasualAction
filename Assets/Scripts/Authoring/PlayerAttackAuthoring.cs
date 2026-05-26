using Unity.Entities;
using UnityEngine;

public class PlayerAttackAuthoring : MonoBehaviour
{
    public float Damage;
    public float Radius;
    public float Cooldown;
    public float Timer;

    class Baker : Baker<PlayerAttackAuthoring>
    {
        public override void Bake(PlayerAttackAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.None);
            AddComponent(entity, new PlayerPoisonCloudSkillComponent
            {
                Damage = authoring.Damage,
                Radius = authoring.Radius,
                Cooldown = authoring.Cooldown,
                Timer = authoring.Timer
            });
        }
    }
}
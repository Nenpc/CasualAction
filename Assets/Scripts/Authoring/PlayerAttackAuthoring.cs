using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAttackAuthoring : MonoBehaviour
{
    [SerializeField] private int m_DefaultSkillId;

    class Baker : Baker<PlayerAttackAuthoring>
    {
        public override void Bake(PlayerAttackAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.None);
            AddComponent(entity, new PlayerDefaultSkillComponent
            {
                SkillID = authoring.m_DefaultSkillId
            });
        }
    }
}
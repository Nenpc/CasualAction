using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAttackAuthoring : MonoBehaviour
{
    public SkillConfig Skill;
    [Range(0, 4)] public int SkillLevel;
    public bool IsEnabled = true;

    public float Damage;
    public float Radius;
    public float Cooldown;
    public float Timer;

    class Baker : Baker<PlayerAttackAuthoring>
    {
        public override void Bake(PlayerAttackAuthoring authoring)
        {
            SkillLevelConfig skillLevelConfig = null;
            if (authoring.Skill != null && authoring.Skill.LevelCount > 0)
            {
                int levelIndex = math.clamp(authoring.SkillLevel, 0, authoring.Skill.LevelCount - 1);
                skillLevelConfig = authoring.Skill.GetLevel(levelIndex);
            }

            var prefabEntity = Entity.Null;
            if (skillLevelConfig != null && skillLevelConfig.Prefab != null)
            {
                prefabEntity = GetEntity(skillLevelConfig.Prefab, TransformUsageFlags.Dynamic);
            }

            var entity = GetEntity(authoring, TransformUsageFlags.None);
            AddComponent(entity, new PlayerPoisonCloudSkillComponent
            {
                Prefab = prefabEntity,
                IsEnabled = authoring.IsEnabled,
                Damage = skillLevelConfig != null ? skillLevelConfig.Damage : authoring.Damage,
                Radius = authoring.Radius,
                Cooldown = skillLevelConfig != null ? skillLevelConfig.Cooldown : authoring.Cooldown,
                Timer = authoring.Timer
            });
        }
    }
}
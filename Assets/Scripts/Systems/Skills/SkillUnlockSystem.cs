using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

public partial struct SkillUnlockSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SkillLibraryComponent>();
        state.RequireForUpdate<SkillDataBuffer>();
        state.RequireForUpdate<SkillUnlockComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var entityManager = state.EntityManager;
        var configEntity = SystemAPI.GetSingletonEntity<SkillLibraryComponent>();
        var skillBuffer = entityManager.GetBuffer<SkillDataBuffer>(configEntity);
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (unlockData, requestEntity) in SystemAPI.Query<RefRO<SkillUnlockComponent>>().WithEntityAccess())
        {
            var request = unlockData.ValueRO;
            if (!SkillDataHelper.TryGetSkillData(skillBuffer, request.SkillId, request.Level, out var skillData))
                continue;

            if (TryUnlockSkill(ecb, skillData))
            {
                ecb.RemoveComponent<SkillUnlockComponent>(requestEntity);
            }
        }

        ecb.Playback(entityManager);
        ecb.Dispose();
    }

    private static bool TryUnlockSkill(EntityCommandBuffer ecb, in SkillDataBuffer skillData)
    {
        switch (GetCharacterSkillType(skillData.SkillNameHash))
        {
            case CharacterSkillType.ConeAttack:
            {
                var skillEntity = ecb.CreateEntity();
                ecb.AddComponent(skillEntity, new CharacterConeAttackSkillComponent
                {
                    Prefab = skillData.Prefab,
                    IsEnabled = true,
                    Cooldown = skillData.Cooldown,
                    Radius = 5f,
                    Angle = 90f,
                    Damage = (int)skillData.Damage
                });
                return true;
            }
            case CharacterSkillType.PoisonCloud:
            {
                var skillEntity = ecb.CreateEntity();
                ecb.AddComponent(skillEntity, new CharacterPoisonCloudSkillComponent
                {
                    Prefab = skillData.Prefab,
                    DefaultSkillId = skillData.SkillId,
                    IsEnabled = true,
                    Damage = skillData.Damage,
                    Radius = 3f,
                    Cooldown = skillData.Cooldown,
                    Timer = 0f
                });
                return true;
            }
            default:
                return false;
        }
    }

    private static CharacterSkillType GetCharacterSkillType(int skillNameHash)
    {
        if (skillNameHash == "ConeSkill".GetHashCode() || skillNameHash == "ConeAttackSkill".GetHashCode())
            return CharacterSkillType.ConeAttack;

        if (skillNameHash == "PoisonSkill".GetHashCode() || skillNameHash == "PoisonCloudSkill".GetHashCode() || skillNameHash == "PoisonCloud".GetHashCode())
            return CharacterSkillType.PoisonCloud;

        return CharacterSkillType.Unknown;
    }

    private enum CharacterSkillType
    {
        Unknown,
        ConeAttack,
        PoisonCloud
    }
}
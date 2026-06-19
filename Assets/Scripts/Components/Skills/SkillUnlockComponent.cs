using Unity.Entities;

public struct SkillUnlockComponent : IComponentData
{
    public int SkillId;
    public int Level;
}

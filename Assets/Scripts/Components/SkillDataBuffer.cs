using Unity.Entities;

public struct SkillDataBuffer : IBufferElementData
{
    public int SkillNameHash;
    public int SkillId;
    public float Damage;
    public float Cooldown;
    public Entity Prefab;
    public int Level;
}

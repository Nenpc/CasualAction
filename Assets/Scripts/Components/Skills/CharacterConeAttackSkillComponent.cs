using Unity.Entities;

public struct CharacterConeAttackSkillComponent : IComponentData
{
    public Entity Prefab;
    public bool   IsEnabled;
    public float  Cooldown;
    public float  Radius;
    public float  Angle;
    public int    Damage;
}
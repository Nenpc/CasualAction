using Unity.Entities;

public struct PlayerConeAttackSkillComponent : IComponentData
{
    public Entity Prefab;
    public bool   IsEnabled;
    public float  Cooldown;
    public float  Radius;
    public float  Angle;
    public int    Damage;
}
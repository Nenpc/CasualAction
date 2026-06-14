using Unity.Entities;

public struct PlayerConeAttackSkillComponent : IComponentData
{
    public Entity Prefab;
    public bool   IsEnabled;
    public float  Cooldown;     // время между использованием
    public float  Radius;
    public float  Angle;        // например 45f
    public int    Damage;
}
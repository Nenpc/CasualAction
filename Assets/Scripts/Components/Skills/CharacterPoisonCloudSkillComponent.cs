using Unity.Entities;

public struct CharacterPoisonCloudSkillComponent : IComponentData
{
    public Entity Prefab;
    public int DefaultSkillId;
    public bool IsEnabled;
    public float Damage;
    public float Radius;
    public float Cooldown;
    public float Timer;
}
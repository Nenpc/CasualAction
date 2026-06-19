using Unity.Entities;

public struct CharacterComponent : IComponentData
{
    public int ID;
    public Entity CharacterPrefab;
    public float MaxHealth;
    public float MoveSpeed;
    public int DefaultSkillId;
}
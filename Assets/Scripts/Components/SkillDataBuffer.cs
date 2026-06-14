using Unity.Entities;

/// <summary>
/// Буфер для хранения информации о скилах из SkillLibraryConfig
/// </summary>
public struct SkillDataBuffer : IBufferElementData
{
    public int SkillNameHash;           // хеш имени скила для быстрого поиска
    public float Damage;
    public float Cooldown;
    public Entity Prefab;
    public int Level;                  // уровень (0-4)
}

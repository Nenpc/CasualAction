using Unity.Entities;

public struct SkillLibraryComponent : IComponentData
{
    public Entity SkillLibrary;
}

public struct EnemyPrefabBuffer : IBufferElementData
{
    public Entity Value;
}
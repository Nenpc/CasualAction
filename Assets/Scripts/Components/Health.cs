using Unity.Entities;

public struct Health : IComponentData
{
    public float CurrentHealth;
    public float MaxHealth;
}
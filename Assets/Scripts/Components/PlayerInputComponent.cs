using Unity.Entities;
using Unity.Mathematics;

public struct PlayerInputComponent : IComponentData
{
    public float2 Move;
    
    public bool PoisonCloudPressed;
    public bool ConeAttackPressed;
    
    // Добавляй сюда новые способности по мере необходимости
}
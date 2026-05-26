using Unity.Entities;

public struct GameplayTimeComponent : IComponentData
{
    public double ElapsedTime;     // время, которое считается только когда игра не на паузе
    public float DeltaTime;        // delta только когда не на паузе
    public bool IsPaused;
}
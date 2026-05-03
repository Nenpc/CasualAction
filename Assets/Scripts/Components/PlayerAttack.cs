using Unity.Entities;

public struct PlayerAttack : IComponentData
{
    public float Damage;           // сколько урона наносить за тик
    public float Radius;           // радиус области урона
    public float Cooldown;         // интервал между атаками (рекомендую 1.0f)
    public float Timer;            // внутренний таймер (не трогай в инспекторе)
}
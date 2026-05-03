using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;

[BurstCompile]
public partial struct PlayerAttackSystem: ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerAttack>();
        state.RequireForUpdate<PlayerTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //Debug.Log("Player Attack System Update");

        float deltaTime = SystemAPI.Time.DeltaTime;

        // Получаем данные игрока один раз
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        var playerTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);
        var playerAttack = SystemAPI.GetComponentRW<PlayerAttack>(playerEntity);

        // Обновляем таймер
        playerAttack.ValueRW.Timer += deltaTime;

        // Если ещё не прошло время кулдауна — выходим
        if (playerAttack.ValueRO.Timer < playerAttack.ValueRO.Cooldown)
            return;

        // Сбрасываем таймер
        playerAttack.ValueRW.Timer = 0f;

        float3 center = playerTransform.Position;
        float radius = playerAttack.ValueRO.Radius;
        float damage = playerAttack.ValueRO.Damage;

        // Перебираем всех врагов и проверяем расстояние
        foreach (var (enemyTransform, health, enemyEntity) in
                 SystemAPI.Query<RefRO<LocalTransform>, RefRW<Health>>()
                          .WithAll<EnemyTag>()
                          .WithEntityAccess())
        {
            float3 enemyPos = enemyTransform.ValueRO.Position;
            float distanceSq = math.distancesq(center, enemyPos);

            // Проверка попадания в круг (используем squared distance — быстрее)
            if (distanceSq <= radius * radius)
            {
                // Наносим урон
                health.ValueRW.CurrentHealth -= damage;
            }
        }
    }
}
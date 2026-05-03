using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct EnemySpawnSystem : ISystem
{
    private float _nextSpawnTime;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ConfigComponent>();
        state.RequireForUpdate<GameState>();

        // Первый спавн через 10 секунд после старта игры
        _nextSpawnTime = 10.0f;
    }

    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameState>();
        if (gameState.IsGameOver)
            return;

        float currentTime = (float)SystemAPI.Time.ElapsedTime;

        // Проверяем, пора ли спавнить новую волну
        if (currentTime < _nextSpawnTime)
            return;

        var config = SystemAPI.GetSingleton<ConfigComponent>();

        var camera = Camera.main;
        if (camera == null) return;

        float orthoSize = camera.orthographicSize;
        float halfWidth = orthoSize * camera.aspect;
        float halfHeight = orthoSize;

        float spawnOffset = 2.0f;

        // Спавним всю волну сразу
        for (int i = 0; i < config.EnemyCount; i++)
        {
            float3 spawnPosition = GetRandomPositionOutsideCamera(
                camera.transform.position, 
                halfWidth, 
                halfHeight, 
                spawnOffset);

            var enemyEntity = state.EntityManager.Instantiate(config.EnemyPrefab);

            state.EntityManager.AddComponent<EnemyTag>(enemyEntity);
            state.EntityManager.AddComponent<CharacterComponent>(enemyEntity);

            state.EntityManager.SetComponentData(enemyEntity, 
                LocalTransform.FromPositionRotationScale(
                    spawnPosition, 
                    quaternion.identity,
                    1f
                ));
        }

        // Планируем следующую волну через 10 секунд
        _nextSpawnTime = currentTime + 10.0f;
    }

    [BurstCompile]
    private float3 GetRandomPositionOutsideCamera(float3 cameraPos, float halfWidth, float halfHeight, float offset)
    {
        int side = UnityEngine.Random.Range(0, 4);

        float x, y;

        switch (side)
        {
            case 0: // Лево
                x = cameraPos.x - halfWidth - offset;
                y = cameraPos.y + UnityEngine.Random.Range(-halfHeight, halfHeight);
                break;

            case 1: // Право
                x = cameraPos.x + halfWidth + offset;
                y = cameraPos.y + UnityEngine.Random.Range(-halfHeight, halfHeight);
                break;

            case 2: // Низ
                x = cameraPos.x + UnityEngine.Random.Range(-halfWidth, halfWidth);
                y = cameraPos.y - halfHeight - offset;
                break;

            default: // Верх
                x = cameraPos.x + UnityEngine.Random.Range(-halfWidth, halfWidth);
                y = cameraPos.y + halfHeight + offset;
                break;
        }

        return new float3(x, y, 0);
    }
}
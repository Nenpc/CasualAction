using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct CharacterSpawnSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CharacterComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;

        var characterData = SystemAPI.GetSingleton<CharacterComponent>();

        var characterEntity = state.EntityManager.Instantiate(characterData.CharacterPrefab);
        
        state.EntityManager.AddComponent<CharacterTag>(characterEntity);
        state.EntityManager.AddComponent<MovementSpeedComponent>(characterEntity);
        state.EntityManager.AddComponent<PlayerInputComponent>(characterEntity);
        state.EntityManager.AddComponent<CharacterDirectionComponent>(characterEntity);
        state.EntityManager.AddComponent<CharacterComponent>(characterEntity);
        state.EntityManager.AddComponent<HealthComponent>(characterEntity);

        state.EntityManager.SetComponentData(characterEntity, new CharacterDirectionComponent
        {
            Direction = new float2(1f, 0f)
        });

        state.EntityManager.AddComponentData(characterEntity, new SkillUnlockComponent
        {
            SkillId = characterData.DefaultSkillId,
            Level = 0
        });

        state.EntityManager.SetComponentData(characterEntity,
            new MovementSpeedComponent()
            {
                Value = characterData.MoveSpeed
            });

        state.EntityManager.SetComponentData(characterEntity,
            new HealthComponent()
            {
                MaxHealth = characterData.MaxHealth,
                CurrentHealth = characterData.MaxHealth
            });

        state.EntityManager.SetComponentData(characterEntity, 
            LocalTransform.FromPositionRotationScale(
                float3.zero, 
                quaternion.identity, 
                1f
            ));
    }
}
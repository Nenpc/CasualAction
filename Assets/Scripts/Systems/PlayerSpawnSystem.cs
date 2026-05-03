using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct PLayerSpawnSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ConfigComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;

        var config = SystemAPI.GetSingleton<ConfigComponent>();

        var characterEntity = state.EntityManager.Instantiate(config.PlayerPrefab);
        
        state.EntityManager.AddComponent<PlayerTag>(characterEntity);
        state.EntityManager.AddComponent<CharacterComponent>(characterEntity);

        state.EntityManager.SetComponentData(characterEntity, 
            LocalTransform.FromPositionRotationScale(
                float3.zero, 
                quaternion.identity, 
                1f
            )); 
    }
}
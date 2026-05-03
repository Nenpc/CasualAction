using Unity.Burst;
using Unity.Entities;

[BurstCompile]
[UpdateAfter(typeof(EnemyDeathSystem))]
public partial struct EnemyCleanupSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        
        foreach (var entity in 
                 SystemAPI.QueryBuilder()
                     .WithAll<DeadTag, EnemyTag>()
                     .Build()
                     .ToEntityArray(state.WorldUpdateAllocator))
        {
            ecb.DestroyEntity(entity);
        }
    }
}
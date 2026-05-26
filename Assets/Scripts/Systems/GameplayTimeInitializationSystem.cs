using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct GameplayTimeInitializationSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<GameplayTimeComponent>(out _))
        {
            Entity entity = state.EntityManager.CreateSingleton(new GameplayTimeComponent
            {
                ElapsedTime = 0.0,
                DeltaTime = 0f
            });
            
            state.EntityManager.SetName(entity, "GameplayTime");
        }
    }
}
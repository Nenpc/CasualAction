using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;

public partial struct PlayerInputSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var input = SystemAPI.GetSingletonRW<PlayerInputComponent>();

        float x = 0f, z = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))    z += 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))  z -= 1f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))  x -= 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) x += 1f;

        input.ValueRW.Move = new float2(x, z);
    }
}
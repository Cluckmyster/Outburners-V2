using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
partial struct GoInGameClientSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
       foreach ((
            RefRO<NetworkId> NetworkId, 
            Entity entity) 
            in SystemAPI.Query<
                RefRO<NetworkId>>().WithNone<NetworkStreamInGame>().WithEntityAccess())
        {
            entityCommandBuffer.AddComponent<NetworkStreamInGame>(entity);
            Debug.Log("Setting Client as In Game");

        }
       entityCommandBuffer.Playback(state.EntityManager);
        Entity rpcEntity = entityCommandBuffer.CreateEntity();
        entityCommandBuffer.AddComponent(rpcEntity, new GoInGameRequestRPC());
        entityCommandBuffer.AddComponent(rpcEntity, new SendRpcCommandRequest());
    }
    public struct GoInGameRequestRPC : IRpcCommand {

    }
}

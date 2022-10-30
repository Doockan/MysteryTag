using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace MysteryTag
{
    public sealed class GameObjectAssetLoader : IAssetLoader
    {
        private struct HandlerData
        {
            public object Address;
            public EcsWorld EcsWorld;
            public int Entity;
        }
        
        private readonly Dictionary<AsyncOperationHandle<GameObject>, HandlerData> _handlers = new Dictionary<AsyncOperationHandle<GameObject>, HandlerData>();
        
        public void LoadAsset(object address, EcsWorld ecsWorld, int entity)
        {
            var handlerData = new HandlerData
                              {
                                  Address = address,
                                  EcsWorld = ecsWorld,
                                  Entity = entity
                              };

            var handle = Addressables.LoadAssetAsync<GameObject>(address);

            if (_handlers.ContainsKey(handle))
            {
                Debug.Log("");
            }
            
            handle.Completed += HandleOnComplete;
            
            _handlers[handle] = handlerData;
        }

        private void HandleOnComplete(AsyncOperationHandle<GameObject> handler)
        {
            var handlerData = _handlers[handler];
            if (handler.Status == AsyncOperationStatus.Succeeded)
            {
                var pool = handlerData.EcsWorld.GetPool<PrefabComponent>();
                ref var component = ref pool.Add(handlerData.Entity);
                component.Value = handler.Result;
            }
            else
            {
                Debug.LogError($"Asset for {handlerData.Address} failed to load.");
            }

            _handlers.Remove(handler);
        }
    }
}

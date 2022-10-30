using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MysteryTag
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        private EcsPool<IsPlayerComponent> _playerComponentPool;
        private EcsPool<PlayerMoveInputComponent> _playerInputComponentPool;
        private EcsPool<PlayerFireInputComponent> _playerFireInputComponent;
        private EcsPool<LoadPrefabComponent> _loadPrefabComponent;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
;           _playerComponentPool = world.GetPool<IsPlayerComponent>();
            _playerInputComponentPool = world.GetPool<PlayerMoveInputComponent>();
            _playerFireInputComponent = world.GetPool<PlayerFireInputComponent>();
            _loadPrefabComponent = world.GetPool<LoadPrefabComponent>();
            
            
            CreatePlayerEntity(world);
        }

        private void CreatePlayerEntity(EcsWorld world)
        {
            var playerEntity = world.NewEntity();
            _playerComponentPool.Add(playerEntity);
            _playerInputComponentPool.Add(playerEntity);
            _playerFireInputComponent.Add(playerEntity);
            ref var loadPrefabComponent = ref _loadPrefabComponent.Add(playerEntity);
            loadPrefabComponent.Value = new AssetReferenceT<GameObject>(AddressablePath.MAIN_SHIP);
        }
    }
}
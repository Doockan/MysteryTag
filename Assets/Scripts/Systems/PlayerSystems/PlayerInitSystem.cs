using Components.LoadAssetComponents;
using Components.PlayerComponents;
using Leopotam.EcsLite;
using Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Systems.PlayerSystems
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsPool<IsPlayerComponent> _playerComponentPool;
        private EcsPool<PlayerMoveInputComponent> _playerInputComponentPool;
        private EcsPool<PlayerFireInputComponent> _playerFireInputComponent;
        private EcsPool<LoadPrefabComponent> _loadPrefabComponent;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
;           _playerComponentPool = _world.GetPool<IsPlayerComponent>();
            _playerInputComponentPool = _world.GetPool<PlayerMoveInputComponent>();
            _playerFireInputComponent = _world.GetPool<PlayerFireInputComponent>();
            _loadPrefabComponent = _world.GetPool<LoadPrefabComponent>();
            
            
            CreatePlayerEntity(_world);
        }

        private void CreatePlayerEntity(EcsWorld world)
        {
            int playerEntity = world.NewEntity();
            _playerComponentPool.Add(playerEntity);
            _playerInputComponentPool.Add(playerEntity);
            _playerFireInputComponent.Add(playerEntity);
            ref LoadPrefabComponent loadPrefabComponent = ref _loadPrefabComponent.Add(playerEntity);
            loadPrefabComponent.Value = new AssetReferenceT<GameObject>(AddressablePath.MAIN_SHIP);
        }
    }
}
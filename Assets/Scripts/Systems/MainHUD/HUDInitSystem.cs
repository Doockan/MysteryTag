using Components.HUDComponents;
using Components.LoadAssetComponents;
using Leopotam.EcsLite;
using Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Systems.MainHUD
{
    public class HUDInitSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsPool<IsMainHUDComponent> _mainHUDComponentPool;
        private EcsPool<LoadPrefabComponent> _loadPrefabComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _mainHUDComponentPool = _world.GetPool<IsMainHUDComponent>();
            _loadPrefabComponentPool = _world.GetPool<LoadPrefabComponent>();

            int entity = _world.NewEntity();
            _mainHUDComponentPool.Add(entity);
            ref LoadPrefabComponent loadPrefabComponent = ref _loadPrefabComponentPool.Add(entity);
            loadPrefabComponent.Value = new AssetReferenceT<GameObject>(AddressablePath.MAIN_HUD);
        }
    }
}
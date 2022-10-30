using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MysteryTag
{
    public class HUDInitSystem : IEcsInitSystem
    {
        private EcsPool<IsMainHUDComponent> _mainHUDComponentPool;
        private EcsPool<LoadPrefabComponent> _loadPrefabComponentPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _mainHUDComponentPool = world.GetPool<IsMainHUDComponent>();
            _loadPrefabComponentPool = world.GetPool<LoadPrefabComponent>();

            var entity = world.NewEntity();
            _mainHUDComponentPool.Add(entity);
            ref var loadPrefabComponent = ref _loadPrefabComponentPool.Add(entity);
            loadPrefabComponent.Value = new AssetReferenceT<GameObject>(AddressablePath.MAIN_HUD);
        }
    }
}
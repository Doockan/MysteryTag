using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MysteryTag
{
    public class MainMenuLoadPrefabsSystem: IEcsInitSystem
    {
        private EcsPool<IsMainMenuCanvas> _isMainMenuCanvasComponentPool;
        private EcsPool<IsLevelViewComponent> _isLevelViewComponentPool;
        private EcsPool<LoadPrefabComponent> _loadPrefabComponentPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            InitPools(world);

            LoadPrefabs(world);
        }

        private void InitPools(EcsWorld world)
        {
            _isMainMenuCanvasComponentPool = world.GetPool<IsMainMenuCanvas>();
            _isLevelViewComponentPool = world.GetPool<IsLevelViewComponent>();
            _loadPrefabComponentPool = world.GetPool<LoadPrefabComponent>();
        }

        private void LoadPrefabs(EcsWorld world)
        {
            var canvasEntity = world.NewEntity();
            _isMainMenuCanvasComponentPool.Add(canvasEntity);
            ref var canvasLoadPrefabComponent = ref _loadPrefabComponentPool.Add(canvasEntity);
            canvasLoadPrefabComponent.Value = new AssetReferenceT<GameObject>(AddressablePath.MAIN_MENU_CANVAS);

            var levelEntity = world.NewEntity();
            _isLevelViewComponentPool.Add(levelEntity);
            ref var levelLoadPrefabComponent = ref _loadPrefabComponentPool.Add(levelEntity);
            levelLoadPrefabComponent.Value = new AssetReferenceT<GameObject>(AddressablePath.MAIN_MENU_LEVEL);
        }
    }
}
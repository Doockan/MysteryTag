using Components.LoadAssetComponents;
using Components.MenuComponents;
using Leopotam.EcsLite;
using Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Systems.MenuSystems
{
    public class MainMenuLoadPrefabsSystem: IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsPool<IsMainMenuCanvas> _isMainMenuCanvasComponentPool;
        private EcsPool<IsLevelViewComponent> _isLevelViewComponentPool;
        private EcsPool<LoadPrefabComponent> _loadPrefabComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _isMainMenuCanvasComponentPool = _world.GetPool<IsMainMenuCanvas>();
            _isLevelViewComponentPool = _world.GetPool<IsLevelViewComponent>();
            _loadPrefabComponentPool = _world.GetPool<LoadPrefabComponent>();
            LoadPrefabs(_world);
        }

        private void LoadPrefabs(EcsWorld world)
        {
            int canvasEntity = world.NewEntity();
            _isMainMenuCanvasComponentPool.Add(canvasEntity);
            ref LoadPrefabComponent canvasLoadPrefabComponent = ref _loadPrefabComponentPool.Add(canvasEntity);
            canvasLoadPrefabComponent.Value = new AssetReferenceT<GameObject>(AddressablePath.MAIN_MENU_CANVAS);

            var levelEntity = world.NewEntity();
            _isLevelViewComponentPool.Add(levelEntity);
            ref LoadPrefabComponent levelLoadPrefabComponent = ref _loadPrefabComponentPool.Add(levelEntity);
            levelLoadPrefabComponent.Value = new AssetReferenceT<GameObject>(AddressablePath.MAIN_MENU_LEVEL);
        }
    }
}
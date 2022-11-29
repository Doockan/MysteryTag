using Components.LoadAssetComponents;
using Leopotam.EcsLite;
using Services;

namespace Systems.LoadAssetSystem
{
    public sealed class LoadPrefabSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<LoadPrefabComponent> _loadPrefabComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<LoadPrefabComponent>().End();
            _loadPrefabComponentPool = _world.GetPool<LoadPrefabComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref LoadPrefabComponent loadPrefabComponent = ref _loadPrefabComponentPool.Get(entity);
                new GameObjectAssetLoader().LoadAsset(loadPrefabComponent.Value.RuntimeKey, systems.GetWorld(), entity);
                _loadPrefabComponentPool.Del(entity);
            }
        }
    }
}
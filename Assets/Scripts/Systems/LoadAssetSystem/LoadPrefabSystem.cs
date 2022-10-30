using Leopotam.EcsLite;


namespace MysteryTag
{
    public sealed class LoadPrefabSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<LoadPrefabComponent> _loadPrefabComponentPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<LoadPrefabComponent>().End();
            _loadPrefabComponentPool = world.GetPool<LoadPrefabComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var loadPrefabComponent = ref _loadPrefabComponentPool.Get(entity);
                new GameObjectAssetLoader().LoadAsset(loadPrefabComponent.Value.RuntimeKey, systems.GetWorld(),
                                                      entity);
                _loadPrefabComponentPool.Del(entity);
            }
        }
    }
}
using Components;
using Components.MenuComponents;
using Leopotam.EcsLite;

namespace Systems.MenuSystems
{
    public sealed class MouseClickRegistrationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<IsClickEventComponent> _clickPool;
        private EcsPool<IsCastUpEventComponent> _castUpPool;
        private EcsPool<IsCastDownEventComponent> _castDownPool;


        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsClickableComponent>().Inc<IsCastDownEventComponent>().Inc<IsCastUpEventComponent>().End();

            _clickPool = _world.GetPool<IsClickEventComponent>();
            _castUpPool = _world.GetPool<IsCastUpEventComponent>();
            _castDownPool = _world.GetPool<IsCastDownEventComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                _castDownPool.Del(entity);
                _castUpPool.Del(entity);
                _clickPool.Add(entity);
            }
        }
    }
}
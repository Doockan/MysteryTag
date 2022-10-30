using Leopotam.EcsLite;

namespace MysteryTag
{
    public sealed class MouseClickRegistrationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<IsClickEventComponent> _clickPool;
        private EcsPool<IsCastUpEventComponent> _castUpPool;
        private EcsPool<IsCastDownEventComponent> _castDownPool;


        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<IsClickableComponent>().Inc<IsCastDownEventComponent>().Inc<IsCastUpEventComponent>().End();

            _clickPool = world.GetPool<IsClickEventComponent>();
            _castUpPool = world.GetPool<IsCastUpEventComponent>();
            _castDownPool = world.GetPool<IsCastDownEventComponent>();
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
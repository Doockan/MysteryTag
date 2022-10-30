using Leopotam.EcsLite;

namespace MysteryTag
{
    public class MissedAsteroidsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<IsPlayerHasDamageRequestComponent> _playerHasDamageRequestComponent;
        private EcsPool<DamageComponent> _damageComponentPool;
        private EcsPool<IsDestroyRequestComponent> _destroyRequesComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsAsteroidsComponent>().Inc<TransformComponent>().End();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _playerHasDamageRequestComponent = _world.GetPool<IsPlayerHasDamageRequestComponent>();
            _damageComponentPool = _world.GetPool<DamageComponent>();
            _destroyRequesComponentPool = _world.GetPool<IsDestroyRequestComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var asteroid in _filter)
            {
                var transformComponent = _transformComponentPool.Get(asteroid);
                if (transformComponent.Value.position.y < -1)
                {
                    _destroyRequesComponentPool.Add(asteroid);

                    var damageRequest = _world.NewEntity();
                    ref var damageComponent = ref _damageComponentPool.Add(damageRequest);
                    damageComponent.Value = 1;
                    _playerHasDamageRequestComponent.Add(damageRequest);
                }
            }
        }
    }
}
using Leopotam.EcsLite;

namespace MysteryTag
{
    public class PlayerHitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<HitComponent> _hitComponentPool;
        private EcsPool<IsDestroyRequestComponent> _destroyRequestComponentPool;
        private EcsPool<IsPlayerHasDamageRequestComponent> _playerHasDamageRequestComponent;
        private EcsPool<DamageComponent> _damageComponent;


        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsPlayerComponent>().Inc<HitComponent>().End();
            _hitComponentPool = _world.GetPool<HitComponent>();
            _destroyRequestComponentPool = _world.GetPool<IsDestroyRequestComponent>();
            _playerHasDamageRequestComponent = _world.GetPool<IsPlayerHasDamageRequestComponent>();
            _damageComponent = _world.GetPool<DamageComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var hitComponent = _hitComponentPool.Get(entity);
                _hitComponentPool.Del(entity);
                var otherGameObject = hitComponent.other;
                var otherTransformView = otherGameObject.GetComponent<TransformView>();
                var otherEntity = otherTransformView.GetEntity;
                _destroyRequestComponentPool.Add(otherEntity);

                var request = _world.NewEntity();
                ref var damageComponent = ref _damageComponent.Add(request);
                damageComponent.Value = 1;
                _playerHasDamageRequestComponent.Add(request);
            }
        }
    }
}
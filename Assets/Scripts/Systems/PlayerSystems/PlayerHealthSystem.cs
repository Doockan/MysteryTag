using Components;
using Components.PlayerComponents;
using Leopotam.EcsLite;

namespace Systems.PlayerSystems
{
    public class PlayerHealthSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _player;
        private EcsFilter _damage;
        private EcsPool<HealthComponent> _healthComponentPool;
        private EcsPool<DamageComponent> _damageComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _player = _world.Filter<IsPlayerComponent>().Inc<HealthComponent>().End();
            _damage = _world.Filter<IsPlayerHasDamageRequestComponent>().Inc<DamageComponent>().End();
            _healthComponentPool = _world.GetPool<HealthComponent>();
            _damageComponentPool = _world.GetPool<DamageComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int damage in _damage)
            {
                DamageComponent damageComponent = _damageComponentPool.Get(damage);
                foreach (var player in _player)
                {
                    ref HealthComponent healthComponent = ref _healthComponentPool.Get(player);
                    healthComponent.Value -= damageComponent.Value;
                }
                _world.DelEntity(damage);
            }
        }
    }
}
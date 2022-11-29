using Components;
using Components.AsteroidsComponents;
using Components.PlayerComponents;
using Leopotam.EcsLite;
using UnityEngine;
using Views;

namespace Systems.PlayerSystems
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
            foreach (int entity in _filter)
            {
                HitComponent hitComponent = _hitComponentPool.Get(entity);
                DestroyOther(hitComponent);
                TakeDamage();
                _hitComponentPool.Del(entity);
            }
        }

        private void TakeDamage()
        {
            int request = _world.NewEntity();
            ref DamageComponent damageComponent = ref _damageComponent.Add(request);
            damageComponent.Value = 1;
            _playerHasDamageRequestComponent.Add(request);
        }

        private void DestroyOther(HitComponent hitComponent)
        {
            TransformView otherTransformView = hitComponent.other.GetComponent<TransformView>();
            int otherEntity = otherTransformView.GetEntity;
            _destroyRequestComponentPool.Add(otherEntity);
        }
    }
}
using Components;
using Components.AsteroidsComponents;
using Components.FireComponents;
using Leopotam.EcsLite;
using UnityEngine;
using Views;

namespace Systems.FireSystems
{
    public class ProjectileHitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<HitComponent> _hitComponentPool;
        private EcsPool<IsFlyingProjectileComponent> _flyingProjecttilePool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<RigidbodyComponent> _rigidbodyComponentPool;
        private EcsPool<IsReadyToFireComponent> _readyToFirePool;
        private EcsPool<IsDestroyRequestComponent> _destroyRequestComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsProjectileComponent>().Inc<HitComponent>().End();
            _hitComponentPool = _world.GetPool<HitComponent>();
            _flyingProjecttilePool = _world.GetPool<IsFlyingProjectileComponent>();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _rigidbodyComponentPool = _world.GetPool<RigidbodyComponent>();
            _readyToFirePool = _world.GetPool<IsReadyToFireComponent>();
            _destroyRequestComponentPool = _world.GetPool<IsDestroyRequestComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                HitComponent hitComponent = _hitComponentPool.Get(entity);
                _hitComponentPool.Del(entity);
                GameObject otherGameObject = hitComponent.other;
                TransformView otherTransformView = otherGameObject.GetComponent<TransformView>();
                int otherEntity = otherTransformView.GetEntity;
                _destroyRequestComponentPool.Add(otherEntity);

                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                ref RigidbodyComponent rigidbodyComponent = ref _rigidbodyComponentPool.Get(entity);
                
                AddToProjectilePool(entity, rigidbodyComponent, transformComponent);
            }
        }

        private void AddToProjectilePool(int entity, RigidbodyComponent rigidbodyComponent,
            TransformComponent transformComponent)
        {
            _flyingProjecttilePool.Del(entity);
            rigidbodyComponent.Value.velocity = Vector3.zero;
            transformComponent.Value.gameObject.SetActive(false);
            transformComponent.Value.position = new Vector3(0, 0, 0);
            _readyToFirePool.Add(entity);
        }
    }
}
using Leopotam.EcsLite;
using UnityEngine;

namespace MysteryTag
{
    public class ProjecttileHitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<HitComponent> _hitComponentPool;
        private EcsPool<IsFlyingProjecttileComponent> _flyingProjecttilePool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<RigidbodyComponent> _rigidbodyComponentPool;
        private EcsPool<IsReadyToFireComponent> _readyToFirePool;
        private EcsPool<IsDestroyRequestComponent> _destroyRequestComponentPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<IsProjecttileComponent>().Inc<HitComponent>().End();
            _hitComponentPool = world.GetPool<HitComponent>();
            _flyingProjecttilePool = world.GetPool<IsFlyingProjecttileComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _rigidbodyComponentPool = world.GetPool<RigidbodyComponent>();
            _readyToFirePool = world.GetPool<IsReadyToFireComponent>();
            _destroyRequestComponentPool = world.GetPool<IsDestroyRequestComponent>();
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

                ref var transformComponent = ref _transformComponentPool.Get(entity);
                ref var rigidbodyComponent = ref _rigidbodyComponentPool.Get(entity);
                _flyingProjecttilePool.Del(entity);
                rigidbodyComponent.Value.velocity = Vector3.zero;
                transformComponent.Value.gameObject.SetActive(false);
                transformComponent.Value.position = new Vector3(0, 0, 0);
                _readyToFirePool.Add(entity);
            }
        }
    }
}
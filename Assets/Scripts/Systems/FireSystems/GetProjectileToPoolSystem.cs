using Components;
using Components.FireComponents;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems.FireSystems
{
    public class GetProjectileToPoolSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<IsReadyToFireComponent> _readyToFirePool;
        private EcsPool<IsFlyingProjectileComponent> _flyingProjectilePool;

        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<RigidbodyComponent> _rigidbodyComponentPool;


        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsProjectileComponent>().Inc<IsFlyingProjectileComponent>().End();
            _readyToFirePool = _world.GetPool<IsReadyToFireComponent>();
            _flyingProjectilePool = _world.GetPool<IsFlyingProjectileComponent>();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _rigidbodyComponentPool = _world.GetPool<RigidbodyComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                ref RigidbodyComponent rigidbodyComponent = ref _rigidbodyComponentPool.Get(entity);

                if (transformComponent.Value.position.y > 10)
                {
                    _flyingProjectilePool.Del(entity);
                    rigidbodyComponent.Value.velocity = Vector3.zero;
                    transformComponent.Value.gameObject.SetActive(false);
                    transformComponent.Value.position = new Vector3(0, 0, 0);
                    _readyToFirePool.Add(entity);
                }
            }
        }
    }
}
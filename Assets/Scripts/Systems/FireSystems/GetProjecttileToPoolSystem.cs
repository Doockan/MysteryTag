using Leopotam.EcsLite;
using UnityEngine;

namespace MysteryTag
{
    public class GetProjecttileToPoolSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<IsReadyToFireComponent> _readyToFirePool;
        private EcsPool<IsFlyingProjecttileComponent> _fluingProjecttilePool;
        
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<RigidbodyComponent> _rigidbodyComponentPool;


        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<IsProjecttileComponent>().Inc<IsFlyingProjecttileComponent>().End();
            _readyToFirePool = world.GetPool<IsReadyToFireComponent>();
            _fluingProjecttilePool = world.GetPool<IsFlyingProjecttileComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _rigidbodyComponentPool = world.GetPool<RigidbodyComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var transformComponent = ref _transformComponentPool.Get(entity);
                ref var rigidbodyComponent = ref _rigidbodyComponentPool.Get(entity);

                if (transformComponent.Value.position.y > 10)
                {
                    _fluingProjecttilePool.Del(entity);
                    rigidbodyComponent.Value.velocity = Vector3.zero;
                    transformComponent.Value.gameObject.SetActive(false);
                    transformComponent.Value.position = new Vector3(0, 0, 0);
                    _readyToFirePool.Add(entity);
                }
            }
        }
    }
}
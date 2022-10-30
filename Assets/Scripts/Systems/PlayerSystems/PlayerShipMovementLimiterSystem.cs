using Leopotam.EcsLite;
using UnityEngine;

namespace MysteryTag
{
    public class PlayerShipMovementLimiterSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<TransformComponent> _transformComponentPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<IsPlayerComponent>().Inc<TransformComponent>().End();
            _transformComponentPool = world.GetPool<TransformComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var transformComponent = ref _transformComponentPool.Get(entity);
                var leftLimiter = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x + 0.25f;
                var rightLimiter = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0f, 0f)).x - 0.25f;
                
                if (transformComponent.Value.position.y <= 0)
                {
                    transformComponent.Value.position = new Vector3(transformComponent.Value.position.x, 0, 0);
                }
                
                if (transformComponent.Value.position.x < leftLimiter)
                {
                    transformComponent.Value.position = new Vector3(leftLimiter, transformComponent.Value.position.y, 0);
                }

                if (transformComponent.Value.position.x > rightLimiter)
                {
                    transformComponent.Value.position = new Vector3(rightLimiter, transformComponent.Value.position.y, 0);
                }
            }
        }
    }
}
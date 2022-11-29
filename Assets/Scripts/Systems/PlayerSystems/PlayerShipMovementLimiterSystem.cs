using Components;
using Components.PlayerComponents;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems.PlayerSystems
{
    public class PlayerShipMovementLimiterSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<TransformComponent> _transformComponentPool;
        private Camera _camera;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsPlayerComponent>().Inc<TransformComponent>().End();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            
            _camera = Camera.main;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                
                float leftLimiter = _camera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x + 0.25f;
                float rightLimiter = _camera.ScreenToWorldPoint(new Vector3(_camera.pixelWidth, 0f, 0f)).x - 0.25f;
                
                CheckFloorLimited(transformComponent);
                
                CheckLeftLimited(transformComponent, leftLimiter);

                CheckRightLimited(transformComponent, rightLimiter);
            }
        }

        private static void CheckRightLimited(TransformComponent transformComponent, float rightLimiter)
        {
            if (transformComponent.Value.position.x > rightLimiter)
                transformComponent.Value.position = new Vector3(rightLimiter, transformComponent.Value.position.y, 0);
        }

        private static void CheckLeftLimited(TransformComponent transformComponent, float leftLimiter)
        {
            if (transformComponent.Value.position.x < leftLimiter)
                transformComponent.Value.position = new Vector3(leftLimiter, transformComponent.Value.position.y, 0);
        }

        private static void CheckFloorLimited(TransformComponent transformComponent)
        {
            if (transformComponent.Value.position.y <= 0)
                transformComponent.Value.position = new Vector3(transformComponent.Value.position.x, 0, 0);
        }
    }
}
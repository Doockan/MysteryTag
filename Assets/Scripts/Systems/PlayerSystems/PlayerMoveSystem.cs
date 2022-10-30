using Leopotam.EcsLite;
using UnityEngine;

namespace MysteryTag
{
    public class PlayerMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<PlayerMoveInputComponent> _playerMoveInputComponentPool;
        private EcsPool<SpeedComponent> _speedComponentPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<IsPlayerComponent>().Inc<PlayerMoveInputComponent>().Inc<TransformComponent>().End();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _playerMoveInputComponentPool = world.GetPool<PlayerMoveInputComponent>();
            _speedComponentPool = world.GetPool<SpeedComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var speedComponent = _speedComponentPool.Get(entity);
                var playerMoveInputComponent = _playerMoveInputComponentPool.Get(entity);
                ref var transformComponent = ref _transformComponentPool.Get(entity);

                transformComponent.Value.position = transformComponent.Value.position +
                                                    new Vector3(
                                                        playerMoveInputComponent.MoveInput.x * speedComponent.Value *
                                                        Time.deltaTime,
                                                        playerMoveInputComponent.MoveInput.y * speedComponent.Value *
                                                        Time.deltaTime,
                                                        0);
            }
        }
    }
}
using Components;
using Components.PlayerComponents;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems.PlayerSystems
{
    public class PlayerMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<PlayerMoveInputComponent> _playerMoveInputComponentPool;
        private EcsPool<SpeedComponent> _speedComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsPlayerComponent>().Inc<PlayerMoveInputComponent>().Inc<TransformComponent>().End();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _playerMoveInputComponentPool = _world.GetPool<PlayerMoveInputComponent>();
            _speedComponentPool = _world.GetPool<SpeedComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                SpeedComponent speedComponent = _speedComponentPool.Get(entity);
                PlayerMoveInputComponent playerMoveInputComponent = _playerMoveInputComponentPool.Get(entity);
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);

                transformComponent.Value.position += new Vector3(
                    playerMoveInputComponent.MoveInput.x * speedComponent.Value *
                    Time.deltaTime,
                    playerMoveInputComponent.MoveInput.y * speedComponent.Value *
                    Time.deltaTime,
                    0);
            }
        }
    }
}
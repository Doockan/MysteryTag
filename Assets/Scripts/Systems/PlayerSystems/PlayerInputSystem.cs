using Components;
using Components.PlayerComponents;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems.PlayerSystems
{
    public class PlayerInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<PlayerMoveInputComponent> _playerMoveInputComponentPool;
        private EcsPool<PlayerFireInputComponent> _playerFireInputComponentPool;
        private EcsPool<IsPausedRequestComponent> _isPausedRequestComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _filter = _world.Filter<IsPlayerComponent>().End();
            _playerMoveInputComponentPool = _world.GetPool<PlayerMoveInputComponent>();
            _playerFireInputComponentPool = _world.GetPool<PlayerFireInputComponent>();
            _isPausedRequestComponentPool = _world.GetPool<IsPausedRequestComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                MovedAxis(entity);

                FireButton(entity);

                PausedButton();
            }
        }

        private void MovedAxis(int entity)
        {
            ref PlayerMoveInputComponent playerMoveInputComponent = ref _playerMoveInputComponentPool.Get(entity);
            playerMoveInputComponent.MoveInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        }

        private void FireButton(int entity)
        {
            ref PlayerFireInputComponent playerFireInputComponent = ref _playerFireInputComponentPool.Get(entity);
            playerFireInputComponent.Fire = Input.GetMouseButton(0);
        }

        private void PausedButton()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                int request = _world.NewEntity();
                _isPausedRequestComponentPool.Add(request);
            }
        }
    }
}
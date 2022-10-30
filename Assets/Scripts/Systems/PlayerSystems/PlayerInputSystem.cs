using Leopotam.EcsLite;
using UnityEngine;

namespace MysteryTag
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
                ref var playerMoveInputComponent = ref _playerMoveInputComponentPool.Get(entity);
                playerMoveInputComponent.MoveInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

                ref var playerFireInputComponent = ref _playerFireInputComponentPool.Get(entity);
                playerFireInputComponent.Fire = Input.GetMouseButton(0);

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    var request = _world.NewEntity();
                    _isPausedRequestComponentPool.Add(request);
                }
            }
        }
    }
}
using Components.FireComponents;
using Components.PlayerComponents;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems.FireSystems
{
    public class FireRequestSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private SharedData _sharedData;
        private EcsPool<PlayerFireInputComponent> _playerFireInputComponentPool;
        private EcsPool<IsFireRequestComponent> _fireRequestComponentPool;

        private float _curTimeout;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>();
            _filter = _world.Filter<PlayerFireInputComponent>().End();
            _playerFireInputComponentPool = _world.GetPool<PlayerFireInputComponent>();
            _fireRequestComponentPool = _world.GetPool<IsFireRequestComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            _curTimeout -= Time.deltaTime;
            foreach (int entity in _filter)
            {
                PlayerFireInputComponent playerFireInputComponent = _playerFireInputComponentPool.Get(entity);
                if (playerFireInputComponent.Fire)
                {
                    if (_curTimeout <= 0f)
                    {
                        _curTimeout = _sharedData.GetMainData.GetShipAttackSpeed;
                        
                        int fireRequest = _world.NewEntity();
                        _fireRequestComponentPool.Add(fireRequest);
                    }
                }
            }
        }
    }
}
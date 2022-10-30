using Leopotam.EcsLite;
using UnityEngine;

namespace MysteryTag
{
    public class PlayerSpawnSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private SharedData _sharedData;
        private EcsPool<PrefabComponent> _gameObjectComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<SpeedComponent> _speedComponentPool;
        private EcsPool<FirePointComponent> _firePointComponentPool;
        private EcsPool<HealthComponent> _healthComponentPool;
        private EcsPool<PlayerScoreComponent> _playerScoreComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>();
            _filter = _world.Filter<IsPlayerComponent>().Inc<PrefabComponent>().End();
            _gameObjectComponentPool = _world.GetPool<PrefabComponent>();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _speedComponentPool = _world.GetPool<SpeedComponent>();
            _firePointComponentPool = _world.GetPool<FirePointComponent>();
            _healthComponentPool = _world.GetPool<HealthComponent>();
            _playerScoreComponentPool = _world.GetPool<PlayerScoreComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                CreateScoreEntity();

                ref var gameObjectComponent = ref _gameObjectComponentPool.Get(entity);
                var gameObject = Object.Instantiate(gameObjectComponent.Value);

                var transformView = gameObject.GetComponent<TransformView>();
                transformView.Init(_world, entity);
                var collisionCheckerView = gameObject.GetComponent<CollisionCheckerView>();
                collisionCheckerView.Init(_world, entity);
                _transformComponentPool.Add(entity).Value = transformView.Transform;
                _firePointComponentPool.Add(entity).Value = gameObject.GetComponent<FirePointView>().Transform;
                _healthComponentPool.Add(entity).Value = _sharedData.GetMainData.GetMaxPlayerHealth;
                _speedComponentPool.Add(entity).Value = _sharedData.GetMainData.GetShipSpeed;

                _gameObjectComponentPool.Del(entity);
            }
        }

        private void CreateScoreEntity()
        {
            var scoreEntity = _world.NewEntity();
            ref var scoreComponent = ref _playerScoreComponentPool.Add(scoreEntity);
            scoreComponent.Value = 0;
        }
    }
}
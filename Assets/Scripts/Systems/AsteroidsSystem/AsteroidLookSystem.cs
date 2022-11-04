﻿using Leopotam.EcsLite;

namespace MysteryTag
{
    public class AsteroidLookSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filterSpawn;
        private EcsFilter _filterDestroy;
        private EcsFilter _filterOver;
        private EcsPool<IsAsteroidSpawnedRequestComponent> _isAsteroidSpawnedRequestComponentPool;
        private EcsPool<IsAsteroidDestroyedRequestComponent> _isAsteroidDestroyedRequestComponentPool;
        private EcsPool<IsLevelAsteroidsIsDestroyComponent> _isLevelAsteroidsIsDestroyComponentPool;
        private EcsPool<IsLevelAsteroidsIsOverComponent> _isLevelAsteroidsIsOverComponentPool;

        private int _spawnedAsteroids;
        private int _destroyedAsteroids;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filterSpawn = _world.Filter<IsAsteroidSpawnedRequestComponent>().End();
            _filterDestroy = _world.Filter<IsAsteroidDestroyedRequestComponent>().End();
            _filterOver = _world.Filter<IsLevelAsteroidsIsOverComponent>().End();
            
            _isAsteroidSpawnedRequestComponentPool = _world.GetPool<IsAsteroidSpawnedRequestComponent>();
            _isAsteroidDestroyedRequestComponentPool = _world.GetPool<IsAsteroidDestroyedRequestComponent>();
            _isLevelAsteroidsIsDestroyComponentPool = _world.GetPool<IsLevelAsteroidsIsDestroyComponent>();
            _isLevelAsteroidsIsOverComponentPool = _world.GetPool<IsLevelAsteroidsIsOverComponent>();

        }

        public void Run(IEcsSystems systems)
        {
            foreach (var request in _filterSpawn)
            {
                _isAsteroidSpawnedRequestComponentPool.Del(request);
                _spawnedAsteroids++;
            }

            foreach (var request in _filterDestroy)
            {
                _isAsteroidDestroyedRequestComponentPool.Del(request);
                _destroyedAsteroids++;
            }

            foreach (var request in _filterOver)
            {
                if (_spawnedAsteroids == _destroyedAsteroids)
                {
                    _isLevelAsteroidsIsDestroyComponentPool.Add(request);
                    _isLevelAsteroidsIsOverComponentPool.Del(request);
                }
            }
            
        }
    }
}
using Components;
using Components.AsteroidsComponents;
using Components.LoadAssetComponents;
using Leopotam.EcsLite;
using UnityEngine;
using Views;

namespace Systems.AsteroidsSystem
{
    public class AsteroidsSpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _sharedData;
        private EcsFilter _smallAsteroidPrefab;
        private EcsFilter _midlAsteroidPrefab;
        private EcsFilter _bigAsteroidPrefab;
        private EcsFilter _loadLevelRequset;
        private EcsPool<IsAsteroidsComponent> _asteroidsComponentPool;
        private EcsPool<PrefabComponent> _prefabComponentPool;
        private EcsPool<IsLevelAsteroidsIsOverComponent> _isLevelAsteroidsIsOverComponentPool;
        private EcsPool<IsAsteroidSpawnedRequestComponent> _isAsteroidSpawnedRequestComponentPool;

        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<RigidbodyComponent> _rigidbodyComponentPool;

        private float _leftWindowEdge;
        private float _rightWindowEdge;
        private float _topSpawnLine;

        private float _asteroidsSpeed;
        private float _asteroidsRespawnSpeed;
        private int _smallAsteroids;
        private int _midlAsteroids;
        private int _bigAsteroids;

        private float _curTimeout;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>();
            InitFilters(_world);
            InitPools(_world);
            Camera camera = Camera.main;
            InitEdge(camera);
            InitLevelSettings();
        }

        public void Run(IEcsSystems systems)
        {
            _curTimeout -= Time.deltaTime;
            if (_curTimeout <= 0)
            {
                _curTimeout = _asteroidsRespawnSpeed;
                SpawnAsteroid();
            }
        }

        private void CreateAsteroidsOverRequest()
        {
            var entity = _world.NewEntity();
            _isLevelAsteroidsIsOverComponentPool.Add(entity);
        }

        private void InitLevelSettings()
        {
            var numLevel = _sharedData.GetMainData.LoadLevelNum;
            _asteroidsSpeed = _sharedData.GetMainData.Levels.Data[numLevel].AsteroidsSpeed;
            _asteroidsRespawnSpeed = _sharedData.GetMainData.Levels.Data[numLevel].AsteroidsRespawnSpeed;
            _smallAsteroids = _sharedData.GetMainData.Levels.Data[numLevel].SmallAsteroids;
            _midlAsteroids = _sharedData.GetMainData.Levels.Data[numLevel].MidlAsteroids;
            _bigAsteroids = _sharedData.GetMainData.Levels.Data[numLevel].BigAsteroids;
        }

        private void InitFilters(EcsWorld world)
        {
            _smallAsteroidPrefab = world.Filter<IsSmallAsteroindPrefabComponent>().Inc<PrefabComponent>().End();
            _midlAsteroidPrefab = world.Filter<IsMidlAsteroindPrefabComponent>().Inc<PrefabComponent>().End();
            _bigAsteroidPrefab = world.Filter<IsBigAsteroindPrefabComponent>().Inc<PrefabComponent>().End();
        }

        private void InitPools(EcsWorld world)
        {
            _asteroidsComponentPool = world.GetPool<IsAsteroidsComponent>();
            _prefabComponentPool = world.GetPool<PrefabComponent>();
            _isLevelAsteroidsIsOverComponentPool = world.GetPool<IsLevelAsteroidsIsOverComponent>();
            _isAsteroidSpawnedRequestComponentPool = _world.GetPool<IsAsteroidSpawnedRequestComponent>();

            _transformComponentPool = world.GetPool<TransformComponent>();
            _rigidbodyComponentPool = world.GetPool<RigidbodyComponent>();
        }

        private void InitEdge(Camera camera)
        {
            _leftWindowEdge = camera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x;
            _rightWindowEdge = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, 0f, 0f)).x;
            _topSpawnLine = camera.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, 0)).y;
        }

        private void SpawnAsteroid()
        {
            int asteroidScale = Random.Range(0, 3);
            switch (asteroidScale)
            {
                case 0:
                    if (_smallAsteroids != 0)
                    {
                        foreach (int smallPrefab in _smallAsteroidPrefab)
                        {
                            int entity = CreateAsteroid(smallPrefab);
                            _asteroidsComponentPool.Add(entity);
                            CreateSpawnedRequest();
                            _smallAsteroids--;
                        }
                    }
                    else
                    {
                        _curTimeout = 0;
                    }
                    break;
                case 1:
                    if (_midlAsteroids != 0)
                    {
                        foreach (int midlPrefab in _midlAsteroidPrefab)
                        {
                            int entity = CreateAsteroid(midlPrefab);
                            _asteroidsComponentPool.Add(entity);
                            CreateSpawnedRequest();
                            _midlAsteroids--;
                        }
                    }
                    else
                    {
                        _curTimeout = 0;
                    }
                    break;
                case 2:
                    if (_bigAsteroids != 0)
                    {
                        foreach (int bigPrefab in _bigAsteroidPrefab)
                        {
                            int entity = CreateAsteroid(bigPrefab);
                            _asteroidsComponentPool.Add(entity);
                            CreateSpawnedRequest();
                            _bigAsteroids--;
                        }
                    }
                    else
                    {
                        _curTimeout = 0;
                    }
                    break;
            }
        }

        private void CreateSpawnedRequest()
        {
            var entirt = _world.NewEntity();
            _isAsteroidSpawnedRequestComponentPool.Add(entirt);
            if (_smallAsteroids == 0 || _midlAsteroids == 0 || _bigAsteroids == 0)
                CreateAsteroidsOverRequest();
        }

        private int CreateAsteroid(int AsteroidPrefab)
        {
            var entity = _world.NewEntity();
            ref var gameObjectComponent = ref _prefabComponentPool.Get(AsteroidPrefab);
            var gameObject = Object.Instantiate(gameObjectComponent.Value);
            var transformView = gameObject.GetComponent<TransformView>();
            transformView.Init(_world, entity);
            var rigidbodyView = gameObject.GetComponent<RigidbodyView>();
            rigidbodyView.Init(_world, entity);
            ref var transformComponent = ref _transformComponentPool.Add(entity);
            transformComponent.Value = transformView.Transform;
            ref var rigidbodyComponent = ref _rigidbodyComponentPool.Add(entity);
            rigidbodyComponent.Value = rigidbodyView.Rigidbody;

            transformComponent.Value.position = new Vector3(Random.Range(_leftWindowEdge, _rightWindowEdge), _topSpawnLine, 0);
            rigidbodyComponent.Value.AddForce(Vector3.down * _asteroidsSpeed, ForceMode.Impulse);
            return entity;
        }
    }
}
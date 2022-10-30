using Leopotam.EcsLite;
using UnityEngine;

namespace MysteryTag
{
    public class FireSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _sharedData;
        private EcsFilter _request;
        private EcsFilter _projecttile;
        private EcsFilter _projecttilePrefab;
        private EcsFilter _firePoint;
        private EcsPool<IsFireRequestComponent> _fireRequestComponentPool;
        private EcsPool<IsProjecttileComponent> _projecttileComponentPool;
        private EcsPool<IsReadyToFireComponent> _readyToFirePool;
        private EcsPool<IsFlyingProjecttileComponent> _flyingProjecttilePool;
        private EcsPool<FirePointComponent> _firePointComponentPool;
        private EcsPool<PrefabComponent> _gameObjectComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<RigidbodyComponent> _rigidbodyComponentPool;

        private Transform _shipFirePoint;
        private float _outOfSightLevel = -15;


        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>();
            InitFilters();
            InitPools();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _firePoint)
            {
                var firePoint = _firePointComponentPool.Get(entity);
                _shipFirePoint = firePoint.Value;
            }
            
            foreach (var request in _request)
            {
                
                if (_projecttile.GetEntitiesCount() == 0)
                {
                    foreach (var factory in _projecttilePrefab)
                    {
                        CreateProjecttile(factory);
                    }
                }

                foreach (var projecttile in _projecttile)
                {
                    ShootProjecttile(projecttile);
                    break;
                }

                _fireRequestComponentPool.Del(request);
            }
        }

        private void InitFilters()
        {
            _request = _world.Filter<IsFireRequestComponent>().End();
            _firePoint = _world.Filter<FirePointComponent>().End();
            _projecttile = _world.Filter<IsProjecttileComponent>().Inc<IsReadyToFireComponent>().Inc<TransformComponent>()
                .Inc<RigidbodyComponent>().End();
            _projecttilePrefab = _world.Filter<IsProjecttilePrefabComponent>().Inc<PrefabComponent>().End();
        }

        private void InitPools()
        {
            _readyToFirePool = _world.GetPool<IsReadyToFireComponent>();
            _flyingProjecttilePool = _world.GetPool<IsFlyingProjecttileComponent>();
            _projecttileComponentPool = _world.GetPool<IsProjecttileComponent>();
            _fireRequestComponentPool = _world.GetPool<IsFireRequestComponent>();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _rigidbodyComponentPool = _world.GetPool<RigidbodyComponent>();
            _firePointComponentPool = _world.GetPool<FirePointComponent>();
            _gameObjectComponentPool = _world.GetPool<PrefabComponent>();
        }

        private void ShootProjecttile(int projecttile)
        {
            ref var transformComponent = ref _transformComponentPool.Get(projecttile);
            ref var rigidbodyComponent = ref _rigidbodyComponentPool.Get(projecttile);

            transformComponent.Value.gameObject.SetActive(true);
            transformComponent.Value.position = _shipFirePoint.position;
            
            rigidbodyComponent.Value.AddForce(Vector3.up * _sharedData.GetMainData.GetProjecttileSpeed, ForceMode.Impulse);

            _readyToFirePool.Del(projecttile);
            _flyingProjecttilePool.Add(projecttile);
        }

        private void CreateProjecttile(int factory)
        {
            var entity = _world.NewEntity();
            ref var gameObjectComponent = ref _gameObjectComponentPool.Get(factory);
            var gameObject = Object.Instantiate(gameObjectComponent.Value);
            var transformView = gameObject.GetComponent<TransformView>();
            transformView.Init(_world, entity);
            var rigidbodyView = gameObject.GetComponent<RigidbodyView>();
            rigidbodyView.Init(_world, entity);
            var collisionCheckerView = gameObject.GetComponent<CollisionCheckerView>();
            collisionCheckerView.Init(_world, entity);
            ref var transformComponent = ref _transformComponentPool.Add(entity);
            transformComponent.Value = transformView.Transform;
            ref var rigidbodyComponent = ref _rigidbodyComponentPool.Add(entity);
            rigidbodyComponent.Value = rigidbodyView.Rigidbody;
            transformComponent.Value.gameObject.SetActive(false);
            rigidbodyComponent.Value.velocity = Vector3.zero;
            transformComponent.Value.position = new Vector3(0, _outOfSightLevel, 0);

            _projecttileComponentPool.Add(entity);
            _readyToFirePool.Add(entity);
        }
    }
}
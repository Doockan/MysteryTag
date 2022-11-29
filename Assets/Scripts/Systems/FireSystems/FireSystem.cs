using Components;
using Components.FireComponents;
using Components.LoadAssetComponents;
using Leopotam.EcsLite;
using UnityEngine;
using Views;

namespace Systems.FireSystems
{
    public class FireSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _sharedData;
        private EcsFilter _request;
        private EcsFilter _projectile;
        private EcsFilter _projectilePrefab;
        private EcsFilter _firePoint;
        private EcsPool<IsFireRequestComponent> _fireRequestComponentPool;
        private EcsPool<IsProjectileComponent> _projectileComponentPool;
        private EcsPool<IsReadyToFireComponent> _readyToFirePool;
        private EcsPool<IsFlyingProjectileComponent> _flyingProjectilePool;
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
            foreach (int entity in _firePoint)
            {
                FirePointComponent firePoint = _firePointComponentPool.Get(entity);
                _shipFirePoint = firePoint.Value;
            }
            
            foreach (int request in _request)
            {
                
                if (_projectile.GetEntitiesCount() == 0)
                {
                    foreach (var factory in _projectilePrefab)
                    {
                        CreateProjectile(factory);
                    }
                }

                foreach (var projectile in _projectile)
                {
                    ShootProjectile(projectile);
                    break;
                }

                _fireRequestComponentPool.Del(request);
            }
        }

        private void InitFilters()
        {
            _request = _world.Filter<IsFireRequestComponent>().End();
            _firePoint = _world.Filter<FirePointComponent>().End();
            _projectile = _world.Filter<IsProjectileComponent>().Inc<IsReadyToFireComponent>().Inc<TransformComponent>()
                .Inc<RigidbodyComponent>().End();
            _projectilePrefab = _world.Filter<IsProjecttilePrefabComponent>().Inc<PrefabComponent>().End();
        }

        private void InitPools()
        {
            _readyToFirePool = _world.GetPool<IsReadyToFireComponent>();
            _flyingProjectilePool = _world.GetPool<IsFlyingProjectileComponent>();
            _projectileComponentPool = _world.GetPool<IsProjectileComponent>();
            _fireRequestComponentPool = _world.GetPool<IsFireRequestComponent>();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _rigidbodyComponentPool = _world.GetPool<RigidbodyComponent>();
            _firePointComponentPool = _world.GetPool<FirePointComponent>();
            _gameObjectComponentPool = _world.GetPool<PrefabComponent>();
        }

        private void ShootProjectile(int projectile)
        {
            ref TransformComponent transformComponent = ref _transformComponentPool.Get(projectile);
            ref RigidbodyComponent rigidbodyComponent = ref _rigidbodyComponentPool.Get(projectile);

            transformComponent.Value.gameObject.SetActive(true);
            transformComponent.Value.position = _shipFirePoint.position;
            
            rigidbodyComponent.Value.AddForce(Vector3.up * _sharedData.GetMainData.GetProjecttileSpeed, ForceMode.Impulse);

            _readyToFirePool.Del(projectile);
            _flyingProjectilePool.Add(projectile);
        }

        private void CreateProjectile(int factory)
        {
            int entity = _world.NewEntity();
            ref PrefabComponent gameObjectComponent = ref _gameObjectComponentPool.Get(factory);
            GameObject gameObject = Object.Instantiate(gameObjectComponent.Value);
            
            TransformView transformView = gameObject.GetComponent<TransformView>();
            RigidbodyView rigidbodyView = gameObject.GetComponent<RigidbodyView>();
            CollisionCheckerView collisionCheckerView = gameObject.GetComponent<CollisionCheckerView>();
            transformView.Init(_world, entity);
            rigidbodyView.Init(_world, entity);
            collisionCheckerView.Init(_world, entity);
            
            ref TransformComponent transformComponent = ref _transformComponentPool.Add(entity);
            transformComponent.Value = transformView.Transform;
            ref RigidbodyComponent rigidbodyComponent = ref _rigidbodyComponentPool.Add(entity);
            rigidbodyComponent.Value = rigidbodyView.Rigidbody;
            
            transformComponent.Value.gameObject.SetActive(false);
            rigidbodyComponent.Value.velocity = Vector3.zero;
            transformComponent.Value.position = new Vector3(0, _outOfSightLevel, 0);

            _projectileComponentPool.Add(entity);
            _readyToFirePool.Add(entity);
        }
    }
}
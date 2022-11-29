using Components.AsteroidsComponents;
using Components.LoadAssetComponents;
using Leopotam.EcsLite;
using Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Systems.AsteroidsSystem
{
    public class AsteroidsInitSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsPool<IsSmallAsteroindPrefabComponent> _smallAsteroidPrefabComponentPool;
        private EcsPool<IsMidlAsteroindPrefabComponent> _midlAsteroidPrefabComponentPool;
        private EcsPool<IsBigAsteroindPrefabComponent> _bigAsteroidPrefabComponentPool;
        private EcsPool<LoadPrefabComponent> _loadPrefabComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _loadPrefabComponentPool = _world.GetPool<LoadPrefabComponent>();
            _smallAsteroidPrefabComponentPool = _world.GetPool<IsSmallAsteroindPrefabComponent>();
            _midlAsteroidPrefabComponentPool = _world.GetPool<IsMidlAsteroindPrefabComponent>();
            _bigAsteroidPrefabComponentPool = _world.GetPool<IsBigAsteroindPrefabComponent>();
            
            InitSmallAsteroid();
            InitMidAsteroid();
            InitBigAsteroid();
       }

        private void InitSmallAsteroid()
        {
            int smallAsteroid = _world.NewEntity();
            _smallAsteroidPrefabComponentPool.Add(smallAsteroid);
            _loadPrefabComponentPool.Add(smallAsteroid).Value = new AssetReferenceT<GameObject>(AddressablePath.SMALL_ASTEROID);
        }

        private void InitMidAsteroid()
        {
            int midlAsteroid = _world.NewEntity();
            _midlAsteroidPrefabComponentPool.Add(midlAsteroid);
            _loadPrefabComponentPool.Add(midlAsteroid).Value = new AssetReferenceT<GameObject>(AddressablePath.MIDL_ASTEROID);
        }

        private void InitBigAsteroid()
        {
            int bigAsteroid = _world.NewEntity();
            _bigAsteroidPrefabComponentPool.Add(bigAsteroid);
            _loadPrefabComponentPool.Add(bigAsteroid).Value = new AssetReferenceT<GameObject>(AddressablePath.BIG_ASTEROID);
        }
    }
}
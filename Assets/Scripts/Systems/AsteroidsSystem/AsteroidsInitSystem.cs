using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MysteryTag
{
    public class AsteroidsInitSystem : IEcsInitSystem
    {
        private EcsPool<IsSmallAsteroindPrefabComponent> _smallAsteroidPrefabComponentPool;
        private EcsPool<IsMidlAsteroindPrefabComponent> _midlAsteroidPrefabComponentPool;
        private EcsPool<IsBigAsteroindPrefabComponent> _bigAsteroidPrefabComponentPool;
        private EcsPool<LoadPrefabComponent> _loadPrefabComponentPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _loadPrefabComponentPool = world.GetPool<LoadPrefabComponent>();
            _smallAsteroidPrefabComponentPool = world.GetPool<IsSmallAsteroindPrefabComponent>();
            _midlAsteroidPrefabComponentPool = world.GetPool<IsMidlAsteroindPrefabComponent>();
            _bigAsteroidPrefabComponentPool = world.GetPool<IsBigAsteroindPrefabComponent>();

            var smallAsteroid = world.NewEntity();
            var midlAsteroid = world.NewEntity();
            var bigAsteroid = world.NewEntity();
            _loadPrefabComponentPool.Add(smallAsteroid).Value = new AssetReferenceT<GameObject>(AddressablePath.SMALL_ASTEROID);
            _loadPrefabComponentPool.Add(midlAsteroid).Value = new AssetReferenceT<GameObject>(AddressablePath.MIDL_ASTEROID);
            _loadPrefabComponentPool.Add(bigAsteroid).Value = new AssetReferenceT<GameObject>(AddressablePath.BIG_ASTEROID);
            _smallAsteroidPrefabComponentPool.Add(smallAsteroid);
            _midlAsteroidPrefabComponentPool.Add(midlAsteroid);
            _bigAsteroidPrefabComponentPool.Add(bigAsteroid);
        }
    }
}
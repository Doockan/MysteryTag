using Components.FireComponents;
using Components.LoadAssetComponents;
using Leopotam.EcsLite;
using Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Systems.FireSystems
{
    public class ProjectileInitSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsPool<LoadPrefabComponent> _loadPrefabComponentPool;
        private EcsPool<IsProjecttilePrefabComponent> _projectileCreatorPrefabPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _loadPrefabComponentPool = _world.GetPool<LoadPrefabComponent>();
            _projectileCreatorPrefabPool = _world.GetPool<IsProjecttilePrefabComponent>();

            int entity = _world.NewEntity();
            _loadPrefabComponentPool.Add(entity).Value = new AssetReferenceT<GameObject>(AddressablePath.PROJECTTILE_PREFAB);
            _projectileCreatorPrefabPool.Add(entity);
        }
    }
}
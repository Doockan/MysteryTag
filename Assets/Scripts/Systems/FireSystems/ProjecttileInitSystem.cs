using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MysteryTag
{
    public class ProjecttileInitSystem : IEcsInitSystem
    {
        private EcsPool<LoadPrefabComponent> _loadPrefabComponentPool;
        private EcsPool<IsProjecttilePrefabComponent> _projecttileCreaterPrefabPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _loadPrefabComponentPool = world.GetPool<LoadPrefabComponent>();
            _projecttileCreaterPrefabPool = world.GetPool<IsProjecttilePrefabComponent>();

            var entity = world.NewEntity();
            _loadPrefabComponentPool.Add(entity).Value = new AssetReferenceT<GameObject>(AddressablePath.PROJECTTILE_PREFAB);
            _projecttileCreaterPrefabPool.Add(entity);
        }
    }
}
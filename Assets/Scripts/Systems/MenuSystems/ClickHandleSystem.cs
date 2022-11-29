using Components;
using Components.MenuComponents;
using Leopotam.EcsLite;
using Services;
using UnityEngine.SceneManagement;

namespace Systems.MenuSystems
{
    public class ClickHandleSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _sharedData;
        private EcsFilter _filter;
        private EcsPool<IsClickEventComponent> _isClickEventComponentPool;
        private EcsPool<LevelComponent> _levelComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>();
            _filter = _world.Filter<LevelComponent>().Inc<IsClickEventComponent>().End();
            _isClickEventComponentPool = _world.GetPool<IsClickEventComponent>();
            _levelComponentPool = _world.GetPool<LevelComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int level in _filter)
            {
                var levelComponent = _levelComponentPool.Get(level);
                if (levelComponent.Status == Availability.NotAvailable)
                {
                    _isClickEventComponentPool.Del(level);
                }
                else
                {
                    _sharedData.GetMainData.LoadLevelNum = levelComponent.Number - 1;
                    SceneManager.LoadScene(1);
                }
            }
        }
    }
}
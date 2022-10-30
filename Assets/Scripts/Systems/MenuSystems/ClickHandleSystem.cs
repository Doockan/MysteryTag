using Leopotam.EcsLite;
using UnityEngine.SceneManagement;

namespace MysteryTag
{
    public class ClickHandleSystem: IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _sharedDatal;
        private EcsFilter _filter;
        private EcsPool<IsClickEventComponent> _isClickEventComponentPool;
        private EcsPool<LevelComponent> _levelComponentPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _sharedDatal = systems.GetShared<SharedData>();
            _filter = world.Filter<LevelComponent>().Inc<IsClickEventComponent>().End();
            _isClickEventComponentPool = world.GetPool<IsClickEventComponent>();
            _levelComponentPool = world.GetPool<LevelComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var level in _filter)
            {
                var levelComponent = _levelComponentPool.Get(level);
                if (levelComponent.Status == Availability.NotAvailable)
                {
                    _isClickEventComponentPool.Del(level);
                }
                else
                {
                    _sharedDatal.GetMainData.LoadLevelNum = levelComponent.Number - 1;
                    SceneManager.LoadScene(1);
                }
            }
        }
    }
}
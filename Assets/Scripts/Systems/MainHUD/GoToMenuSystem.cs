using Components;
using Components.HUDComponents;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems.MainHUD
{
    public class GoToMenuSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<IsClickEventComponent> _isClickEventComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsGoToMenuButtonComponent>().Inc<IsClickEventComponent>().End();
            _isClickEventComponentPool = _world.GetPool<IsClickEventComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var button in _filter)
            {
                _isClickEventComponentPool.Del(button);
                Time.timeScale = 1f;
                SceneManager.LoadScene(0);
            }
        }
    }
}
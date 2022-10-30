using Leopotam.EcsLite;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace MysteryTag
{
    public class GoToMenuSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<IsClickEventComponent> _isClickEventComponentPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<IsGoToMenuButtonComponent>().Inc<IsClickEventComponent>().End();
            _isClickEventComponentPool = world.GetPool<IsClickEventComponent>();
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
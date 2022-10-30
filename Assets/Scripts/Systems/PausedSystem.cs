using Leopotam.EcsLite;
using UnityEngine;

namespace MysteryTag
{
    public class PausedSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _request;
        private EcsFilter _window;
        private EcsFilter _goToMenuButton;
        private EcsPool<IsPausedRequestComponent> _isPausedRequestComponentPool;
        private EcsPool<WindowComponent> _windowComponentPool;
        private EcsPool<ButtonComponent> _buttonComponentPool;

        private bool _isPaused;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _request = world.Filter<IsPausedRequestComponent>().End();
            _window = world.Filter<IsPausedWindowComponent>().Inc<WindowComponent>().End();
            _goToMenuButton = world.Filter<IsGoToMenuButtonComponent>().Inc<ButtonComponent>().End();
            _isPausedRequestComponentPool = world.GetPool<IsPausedRequestComponent>();
            _windowComponentPool = world.GetPool<WindowComponent>();
            _buttonComponentPool = world.GetPool<ButtonComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var request in _request)
            {
                _isPaused = !_isPaused;
                Time.timeScale = _isPaused? 0f : 1f;
                foreach (var window in _window)
                {
                    ref var windowComponent = ref _windowComponentPool.Get(window);
                    windowComponent.Value.SetActive(_isPaused);
                }
                foreach (var button in _goToMenuButton)
                {
                    var buttonComponent = _buttonComponentPool.Get(button);
                    buttonComponent.Value.gameObject.SetActive(true);
                }
                _isPausedRequestComponentPool.Del(request);
            }
        }
    }
}
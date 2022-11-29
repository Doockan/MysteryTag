using Components;
using Components.HUDComponents;
using Components.PlayerComponents;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class GameOverSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsFilter _gameOverWindow;
        private EcsFilter _goToMenuButton;
        private EcsPool<HealthComponent> _healthComponentPool;
        private EcsPool<WindowComponent> _windowComponentPool;
        private EcsPool<ButtonComponent> _buttonComponentPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<IsPlayerComponent>().Inc<HealthComponent>().End();
            _gameOverWindow = world.Filter<IsGameOverWindowComponent>().Inc<WindowComponent>().End();
            _goToMenuButton = world.Filter<IsGoToMenuButtonComponent>().Inc<ButtonComponent>().End();
            _healthComponentPool = world.GetPool<HealthComponent>();
            _windowComponentPool = world.GetPool<WindowComponent>();
            _buttonComponentPool = world.GetPool<ButtonComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                HealthComponent healthComponent = _healthComponentPool.Get(entity);
                if (healthComponent.Value <= 0)
                {
                    Time.timeScale = 0f;
                    ShowGameOverWindow();

                    ShowGoToMenuButton();
                }
            }
        }

        private void ShowGoToMenuButton()
        {
            foreach (var button in _goToMenuButton)
            {
                var buttonComponent = _buttonComponentPool.Get(button);
                buttonComponent.Value.gameObject.SetActive(true);
            }
        }

        private void ShowGameOverWindow()
        {
            foreach (var window in _gameOverWindow)
            {
                var gameOverWindowComponent = _windowComponentPool.Get(window);
                gameOverWindowComponent.Value.SetActive(true);
            }
        }
    }
}
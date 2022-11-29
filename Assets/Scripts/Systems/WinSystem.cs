using Components;
using Components.HUDComponents;
using Leopotam.EcsLite;
using Services;
using Systems.SaveLoadSystems;
using UnityEngine;

namespace Systems
{
    public class WinSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _sharedData;
        private EcsFilter _levelComplete;
        private EcsFilter _winWindow;
        private EcsFilter _goToMenuButton;
        private EcsPool<IsLevelAsteroidsIsDestroyComponent> _isLevelAsteroidsIsDestroyComponentPool;
        private EcsPool<WindowComponent> _windowComponentPool;
        private EcsPool<ButtonComponent> _buttonComponentPool;
        private EcsPool<IsSaveProgressRequestComponent> _isSaveProgressPool;


   

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>();
            _levelComplete = _world.Filter<IsLevelAsteroidsIsDestroyComponent>().End();
            _winWindow = _world.Filter<IsWinWindowComponent>().Inc<WindowComponent>().End();
            _goToMenuButton = _world.Filter<IsGoToMenuButtonComponent>().Inc<ButtonComponent>().End();
            _isLevelAsteroidsIsDestroyComponentPool = _world.GetPool<IsLevelAsteroidsIsDestroyComponent>();
            _windowComponentPool = _world.GetPool<WindowComponent>();
            _buttonComponentPool = _world.GetPool<ButtonComponent>();
            _isSaveProgressPool = _world.GetPool<IsSaveProgressRequestComponent>();
            
        }

        public void Run(IEcsSystems systems)
        {
            ShowWinWindows();
        }

        private void ShowWinWindows()
        {
            foreach (var request in _levelComplete)
            {
                Time.timeScale = 0f;
                CreateSaveRequest();
                ShowWonWindow();
                ShowGoToMenuButton();
                
                _isLevelAsteroidsIsDestroyComponentPool.Del(request);
            }
        }

        private void ShowGoToMenuButton()
        {
            foreach (int button in _goToMenuButton)
            {
                ButtonComponent buttonComponent = _buttonComponentPool.Get(button);
                buttonComponent.Value.gameObject.SetActive(true);
            }
        }

        private void ShowWonWindow()
        {
            foreach (int window in _winWindow)
            {
                WindowComponent winWindowComponent = _windowComponentPool.Get(window);
                winWindowComponent.Value.SetActive(true);
            }
        }

        private void CreateSaveRequest()
        {
            var request = _world.NewEntity();
            _isSaveProgressPool.Add(request);
        }
    }
}
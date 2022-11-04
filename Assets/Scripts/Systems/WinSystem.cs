using Leopotam.EcsLite;
using UnityEngine;

namespace MysteryTag
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


        private SaveData _saveData;
        private int _loadLevelNum;

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
            
            _saveData = SaveSystem.Load();
            _loadLevelNum = _sharedData.GetMainData.LoadLevelNum;
        }

        public void Run(IEcsSystems systems)
        {
            _saveData.Levels[_loadLevelNum].Available = Availability.Passed;
            _saveData.Levels[_loadLevelNum + 1].Available = Availability.Available;
            
            SaveSystem.Save(_saveData);
            
            foreach (var request in _levelComplete)
            {
                Time.timeScale = 0f;
                foreach (var window in _winWindow)
                {
                    var winWindowComponent = _windowComponentPool.Get(window);
                    winWindowComponent.Value.SetActive(true);
                }

                foreach (var button in _goToMenuButton)
                {
                    var buttonComponent = _buttonComponentPool.Get(button);
                    buttonComponent.Value.gameObject.SetActive(true);
                }
            }
        }
    }
}
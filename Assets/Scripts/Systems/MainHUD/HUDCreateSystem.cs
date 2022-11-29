using Components;
using Components.HUDComponents;
using Components.LoadAssetComponents;
using Leopotam.EcsLite;
using UnityEngine;
using Views;

namespace Systems.MainHUD
{
    public class HUDCreateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<PrefabComponent> _gameObjectComponentPool;
        private EcsPool<IsHealthHUDComponent> _isHealthHUDComponentPool;
        private EcsPool<IsScoreHUDComponent> _isScoreHUDComponentPool;
        private EcsPool<TextComponent> _textComponentPool;
        private EcsPool<IsPausedWindowComponent> _isPausedWindowComponentPool;
        private EcsPool<IsGameOverWindowComponent> _isGameOverWindowComponentPool;
        private EcsPool<IsGoToMenuButtonComponent> _isGoToMenuButtonComponentPool;
        private EcsPool<WindowComponent> _windowComponentPool;
        private EcsPool<ButtonComponent> _buttonComponentPool;
        private EcsPool<IsWinWindowComponent> _isWinWindowComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsMainHUDComponent>().Inc<PrefabComponent>().End();
            _gameObjectComponentPool = _world.GetPool<PrefabComponent>();
            _isHealthHUDComponentPool = _world.GetPool<IsHealthHUDComponent>();
            _isScoreHUDComponentPool = _world.GetPool<IsScoreHUDComponent>();
            _textComponentPool = _world.GetPool<TextComponent>();
            _isPausedWindowComponentPool = _world.GetPool<IsPausedWindowComponent>();
            _isGameOverWindowComponentPool = _world.GetPool<IsGameOverWindowComponent>();
            _isWinWindowComponentPool = _world.GetPool<IsWinWindowComponent>();
            _isGoToMenuButtonComponentPool = _world.GetPool<IsGoToMenuButtonComponent>();
            _windowComponentPool = _world.GetPool<WindowComponent>();
            _buttonComponentPool = _world.GetPool<ButtonComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref PrefabComponent gameObjectComponent = ref _gameObjectComponentPool.Get(entity);
                GameObject hudGameObject = Object.Instantiate(gameObjectComponent.Value);

                InitHpBar(hudGameObject);
                InitScoreBar(hudGameObject);
                InitPauseButton(hudGameObject);
                InitGameOverWindow(hudGameObject);
                InitWonWindow(hudGameObject);
                InitGoToMenuButton(hudGameObject);
                
                _gameObjectComponentPool.Del(entity);
            }
        }

        private void InitGoToMenuButton(GameObject gameObject)
        {
            int menuButtonEntity = _world.NewEntity();
            _isGoToMenuButtonComponentPool.Add(menuButtonEntity);
            ref ButtonComponent goToMenuButtonComponent = ref _buttonComponentPool.Add(menuButtonEntity);
            ButtonView goToMenuButtonView = gameObject.GetComponent<MainHUDView>().MenuButton.GetComponent<ButtonView>();
            goToMenuButtonView.Init(_world, menuButtonEntity);
            goToMenuButtonComponent.Value = goToMenuButtonView;
        }

        private void InitWonWindow(GameObject gameObject)
        {
            int winEntity = _world.NewEntity();
            _isWinWindowComponentPool.Add(winEntity);
            ref WindowComponent winComponent = ref _windowComponentPool.Add(winEntity);
            winComponent.Value = gameObject.GetComponent<MainHUDView>().WinWindow;
        }

        private void InitGameOverWindow(GameObject gameObject)
        {
            int gameOverEntity = _world.NewEntity();
            _isGameOverWindowComponentPool.Add(gameOverEntity);
            ref WindowComponent gameOverComponent = ref _windowComponentPool.Add(gameOverEntity);
            gameOverComponent.Value = gameObject.GetComponent<MainHUDView>().GameOverWindow;
        }

        private void InitPauseButton(GameObject gameObject)
        {
            int pausedEntity = _world.NewEntity();
            _isPausedWindowComponentPool.Add(pausedEntity);
            ref WindowComponent pausedComponent = ref _windowComponentPool.Add(pausedEntity);
            pausedComponent.Value = gameObject.GetComponent<MainHUDView>().PausedWindow;
        }

        private void InitScoreBar(GameObject gameObject)
        {
            int scoreEntity = _world.NewEntity();
            _isScoreHUDComponentPool.Add(scoreEntity);
            ref TextComponent scoreTextComponent = ref _textComponentPool.Add(scoreEntity);
            scoreTextComponent.Value = gameObject.GetComponent<MainHUDView>().Score;
        }

        private void InitHpBar(GameObject gameObject)
        {
            int hpEntity = _world.NewEntity();
            _isHealthHUDComponentPool.Add(hpEntity);
            ref TextComponent hpTextComponent = ref _textComponentPool.Add(hpEntity);
            hpTextComponent.Value = gameObject.GetComponent<MainHUDView>().Health;
        }
    }
}
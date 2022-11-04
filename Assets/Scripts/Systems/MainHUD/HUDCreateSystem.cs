using Leopotam.EcsLite;
using UnityEngine;

namespace MysteryTag
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
                ref var gameObjectComponent = ref _gameObjectComponentPool.Get(entity);
                var gameObject = Object.Instantiate(gameObjectComponent.Value);

                var hpEntity = _world.NewEntity();
                _isHealthHUDComponentPool.Add(hpEntity);
                ref var hpTextComponent = ref _textComponentPool.Add(hpEntity);
                hpTextComponent.Value = gameObject.GetComponent<MainHUDView>().Health;

                var scoreEntity = _world.NewEntity();
                _isScoreHUDComponentPool.Add(scoreEntity);
                ref var scoreTextComponent = ref _textComponentPool.Add(scoreEntity);
                scoreTextComponent.Value = gameObject.GetComponent<MainHUDView>().Score;

                var pausedEntity = _world.NewEntity();
                _isPausedWindowComponentPool.Add(pausedEntity);
                ref var pausedComponent = ref _windowComponentPool.Add(pausedEntity);
                pausedComponent.Value = gameObject.GetComponent<MainHUDView>().PausedWindow;

                var gameOverEntity = _world.NewEntity();
                _isGameOverWindowComponentPool.Add(gameOverEntity);
                ref var gameOverComponent = ref _windowComponentPool.Add(gameOverEntity);
                gameOverComponent.Value = gameObject.GetComponent<MainHUDView>().GameOverWindow;
                
                var winEntity = _world.NewEntity();
                _isWinWindowComponentPool.Add(winEntity);
                ref var winComponent = ref _windowComponentPool.Add(winEntity);
                winComponent.Value = gameObject.GetComponent<MainHUDView>().WinWindow;

                var menuButtonEntity = _world.NewEntity();
                _isGoToMenuButtonComponentPool.Add(menuButtonEntity);
                ref var goToMenuButtonComponent = ref _buttonComponentPool.Add(menuButtonEntity);
                var goToMenuButtonView = gameObject.GetComponent<MainHUDView>().MenuButton.GetComponent<ButtonView>();
                goToMenuButtonView.Init(_world, menuButtonEntity);
                goToMenuButtonComponent.Value = goToMenuButtonView;
                
                _gameObjectComponentPool.Del(entity);
            }
        }
    }
}
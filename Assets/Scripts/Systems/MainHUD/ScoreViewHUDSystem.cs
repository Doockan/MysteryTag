using Leopotam.EcsLite;

namespace MysteryTag
{
    public class ScoreViewHUDSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _viewScore;
        private EcsFilter _playerScore;
        private EcsPool<TextComponent> _textComponentPool;
        private EcsPool<PlayerScoreComponent> _playerScoreComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _viewScore = _world.Filter<IsScoreHUDComponent>().Inc<TextComponent>().End();
            _playerScore = _world.Filter<PlayerScoreComponent>().End();
            _textComponentPool = _world.GetPool<TextComponent>();
            _playerScoreComponentPool = _world.GetPool<PlayerScoreComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var score in _playerScore)
            {
                var scoreComponent = _playerScoreComponentPool.Get(score);
                foreach (var view in _viewScore)
                {
                    var viewComponent = _textComponentPool.Get(view);
                    viewComponent.Value.text = $"Score: {scoreComponent.Value}";
                }
            }
        }
    }
}
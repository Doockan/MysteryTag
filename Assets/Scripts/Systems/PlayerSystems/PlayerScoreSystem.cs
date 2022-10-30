using Leopotam.EcsLite;

namespace MysteryTag
{
    public class PlayerScoreSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _request;
        private EcsFilter _playerScore;
        private EcsPool<PlayerScoreComponent> _playerScoreComponentPool;
        private EcsPool<ScoreRequestComponent> _scoreRequestComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _request = _world.Filter<ScoreRequestComponent>().End();
            _playerScore = _world.Filter<PlayerScoreComponent>().End();
            _playerScoreComponentPool = _world.GetPool<PlayerScoreComponent>();
            _scoreRequestComponentPool = _world.GetPool<ScoreRequestComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var request in _request)
            {
                var requestComponent = _scoreRequestComponentPool.Get(request);
                foreach (var playerScore in _playerScore)
                {
                    ref var playerScoreComponent = ref _playerScoreComponentPool.Get(playerScore);
                    playerScoreComponent.Value += requestComponent.Value;
                }
                _world.DelEntity(request);
            }
        }
    }
}
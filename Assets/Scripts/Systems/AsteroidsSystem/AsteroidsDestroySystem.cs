using Leopotam.EcsLite;

namespace MysteryTag
{
    public class AsteroidsDestroySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<ScoreRequestComponent> _scoreRequestComponentPool;
        private EcsPool<IsAsteroidDestroyedRequestComponent> _isAsteroidDestroyedRequestComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsDestroyRequestComponent>().Inc<IsAsteroidsComponent>().End();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _scoreRequestComponentPool = _world.GetPool<ScoreRequestComponent>();
            _isAsteroidDestroyedRequestComponentPool = _world.GetPool<IsAsteroidDestroyedRequestComponent>();

        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var transformComponent = ref _transformComponentPool.Get(entity);
                var gameObject = transformComponent.Value.gameObject;
                var transformView = gameObject.GetComponent<TransformView>();
                transformView.Destroy();
                CreateScoreRequest();
                CreateDestroyRequest();
                _world.DelEntity(entity);
            }
        }

        private void CreateDestroyRequest()
        {
            var entity = _world.NewEntity();
            _isAsteroidDestroyedRequestComponentPool.Add(entity);
        }

        private void CreateScoreRequest()
        {
            var scoreRequest = _world.NewEntity();
            ref var scoreComponent = ref _scoreRequestComponentPool.Add(scoreRequest);
            scoreComponent.Value = 10;
        }
    }
}
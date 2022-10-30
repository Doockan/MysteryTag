using Leopotam.EcsLite;

namespace MysteryTag
{
    public class AsteroidsDestroySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<ScoreRequestComponent> _scoreRequestComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsDestroyRequestComponent>().Inc<IsAsteroidsComponent>().End();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _scoreRequestComponentPool = _world.GetPool<ScoreRequestComponent>();

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
                _world.DelEntity(entity);
            }
        }

        private void CreateScoreRequest()
        {
            var scoreRequest = _world.NewEntity();
            ref var scoreComponent = ref _scoreRequestComponentPool.Add(scoreRequest);
            scoreComponent.Value = 10;
        }
    }
}
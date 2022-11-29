using Components;
using Components.AsteroidsComponents;
using Leopotam.EcsLite;
using Views;

namespace Systems.AsteroidsSystem
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
                DestroyGameObject(entity);
                CreateDestroyRequest();
                CreateScoreRequest();

                _world.DelEntity(entity);
            }
        }

        private void DestroyGameObject(int entity)
        {
            ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
            transformComponent.Value.gameObject
                .GetComponent<TransformView>()
                .Destroy();
        }

        private void CreateDestroyRequest()
        {
            int entity = _world.NewEntity();
            _isAsteroidDestroyedRequestComponentPool.Add(entity);
        }

        private void CreateScoreRequest()
        {
            int scoreRequest = _world.NewEntity();
            ref ScoreRequestComponent scoreComponent = ref _scoreRequestComponentPool.Add(scoreRequest);
            scoreComponent.Value = 10;
        }
    }
}
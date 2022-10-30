using Leopotam.EcsLite;

namespace MysteryTag
{
    public class HealthViewHUDSystem: IEcsInitSystem, IEcsRunSystem

    {
        private EcsFilter _viewHealth;
        private EcsFilter _playerHealth;
        private EcsPool<TextComponent> _textComponentPool;
        private EcsPool<HealthComponent> _healthComponentPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _viewHealth = world.Filter<IsHealthHUDComponent>().Inc<TextComponent>().End();
            _playerHealth = world.Filter<IsPlayerComponent>().Inc<HealthComponent>().End();
            _textComponentPool = world.GetPool<TextComponent>();
            _healthComponentPool = world.GetPool<HealthComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var playerHealth in _playerHealth)
            {
                var playerHealthComponent = _healthComponentPool.Get(playerHealth);
                foreach (var viewHealth in _viewHealth)
                {
                    ref var textComponent = ref _textComponentPool.Get(viewHealth);
                    textComponent.Value.text = $"HP: {playerHealthComponent.Value}";
                }
            }
        }
    }
}
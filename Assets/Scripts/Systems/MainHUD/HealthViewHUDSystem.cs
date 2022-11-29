using Components;
using Components.HUDComponents;
using Components.PlayerComponents;
using Leopotam.EcsLite;

namespace Systems.MainHUD
{
    public class HealthViewHUDSystem: IEcsInitSystem, IEcsRunSystem

    {
        private EcsWorld _world;
        private EcsFilter _viewHealth;
        private EcsFilter _playerHealth;
        private EcsPool<TextComponent> _textComponentPool;
        private EcsPool<HealthComponent> _healthComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _viewHealth = _world.Filter<IsHealthHUDComponent>().Inc<TextComponent>().End();
            _playerHealth = _world.Filter<IsPlayerComponent>().Inc<HealthComponent>().End();
            _textComponentPool = _world.GetPool<TextComponent>();
            _healthComponentPool = _world.GetPool<HealthComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int playerHealth in _playerHealth)
            {
                HealthComponent playerHealthComponent = _healthComponentPool.Get(playerHealth);
                foreach (int viewHealth in _viewHealth)
                {
                    ref TextComponent textComponent = ref _textComponentPool.Get(viewHealth);
                    textComponent.Value.text = $"HP: {playerHealthComponent.Value}";
                }
            }
        }
    }
}
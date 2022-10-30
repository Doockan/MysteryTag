using Leopotam.EcsLite;
using UnityEngine;


namespace MysteryTag
{
    public sealed class EcsStartup : MonoBehaviour
    {
        private EcsSystems _systems;
        private bool _hasInitCompleted = false;

        private async void Start()
        {
            var sharedData = new SharedData();
            await sharedData.Init();
            var ecsWorld = new EcsWorld();

            _systems = new EcsSystems(ecsWorld, sharedData);
            
            _systems
                .Add(new LoadPrefabSystem())
                .Add(new PlayerInitSystem())
                .Add(new ProjecttileInitSystem())
                .Add(new AsteroidsInitSystem())
                .Add(new HUDInitSystem())
                .Add(new HUDCreateSystem())
                .Add(new PlayerSpawnSystem())
                .Add(new PlayerInputSystem())
                .Add(new FireRequestSystem())
                .Add(new ProjecttileHitSystem())
                .Add(new GetProjecttileToPoolSystem())
                .Add(new FireSystem())
                .Add(new PlayerMoveSystem())
                .Add(new PlayerShipMovementLimiterSystem())
                .Add(new AsteroidsSpawnSystem())
                .Add(new MissedAsteroidsSystem())
                .Add(new AsteroidsDestroySystem())
                .Add(new PlayerScoreSystem())
                .Add(new PlayerHitSystem())
                .Add(new PlayerHealthSystem())
                .Add(new GameOverSystem())
                .Add(new PausedSystem())
                .Add(new HealthViewHUDSystem())
                .Add(new ScoreViewHUDSystem())
                .Add(new GoToMenuSystem());


            _systems
                .AddWorld (new EcsWorld (), "DEBAG")
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem("DEBAG"));
            
            _systems.Init();
            _hasInitCompleted = true;
        }
        private void Update()
        {
            if (_hasInitCompleted)
            {
                _systems.Run();
            }
        }
        private void OnDestroy()
        {
            _systems.Destroy();
            _systems.GetWorld().Destroy();
        }
    }
}
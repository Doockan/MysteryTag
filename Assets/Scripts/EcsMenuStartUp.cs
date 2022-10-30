using Leopotam.EcsLite;
using UnityEngine;


namespace MysteryTag
{
    public sealed class EcsMenuStartUp : MonoBehaviour
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
                .Add(new MainMenuLoadPrefabsSystem())
                .Add(new MainMenuInitSystem())
                .Add(new RayCastDownProcessingSystem())
                .Add(new RayCastUpProcessingSystem())
                .Add(new MouseClickRegistrationSystem())
                .Add(new ClickHandleSystem());
            
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
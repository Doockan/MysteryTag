using System.Collections.Generic;
using System.IO;
using Leopotam.EcsLite;
using UnityEngine;

namespace MysteryTag
{
    public class MainMenuInitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _sharedData;
        private EcsFilter _mainCanvasPrefab;
        private EcsFilter _levelPrefab;
        private EcsPool<PrefabComponent> _prefabComponent;
        private EcsPool<IsClickableComponent> _isClickableComponentPool;
        private EcsPool<LevelComponent> _levelComponentPool;

        private SaveData _saveData;
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>();

            _mainCanvasPrefab = _world.Filter<IsMainMenuCanvas>().Inc<PrefabComponent>().End();
            _levelPrefab = _world.Filter<IsLevelViewComponent>().Inc<PrefabComponent>().End();

            _prefabComponent = _world.GetPool<PrefabComponent>();
            _isClickableComponentPool = _world.GetPool<IsClickableComponent>();
            _levelComponentPool = _world.GetPool<LevelComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var canvas in _mainCanvasPrefab)
            {
                var canvasPrefabComponent = _prefabComponent.Get(canvas);
                var parent = Object.Instantiate(canvasPrefabComponent.Value);
                _prefabComponent.Del(canvas);
                
                if (File.Exists(Application.persistentDataPath + "/Save.json"))
                {
                    _saveData = SaveSystem.Load();
                }
                else
                {
                    _saveData = InitStartData();
                    SaveSystem.Save(_saveData);
                }
                
                CreateLevelView(parent);
            }
        }

        private SaveData InitStartData()
        {
            SaveData saveData = new SaveData();
            List<Level> levels = new List<Level>();

            for (int i = 0; i < _sharedData.GetMainData.Levels.Data.Length; i++)
            {
                if (i == 0) levels.Add(new Level {Available = Availability.Available});
                levels.Add(new Level(){Available = Availability.NotAvailable});
            }

            saveData.Levels = levels;
            return saveData;
        }

        private void CreateLevelView(GameObject parent)
        {
            int i = 0;
            foreach (var levelLength in _sharedData.GetMainData.Levels.Data)
            {
                foreach (var prefab in _levelPrefab)
                {
                    var levelPrefabComponent = _prefabComponent.Get(prefab);
                    var gameObject = Object.Instantiate(levelPrefabComponent.Value, parent.transform);
                    var objectView = gameObject.GetComponent<ObjectView>();
                    var levelInfo = gameObject.GetComponent<LevelView>();

                    var entity = _world.NewEntity();
                    _isClickableComponentPool.Add(entity);
                    objectView.Init(_world, entity);
                    ref var levelComponent = ref _levelComponentPool.Add(entity);
                    levelComponent.Number = i + 1;
                    levelInfo.LevelNumber.text = levelComponent.Number.ToString();
                    
                    
                    levelComponent.Status = _saveData.Levels[i].Available;
                    levelInfo.LevelAvailability.text = _saveData.Levels[i].Available.ToString();
                    i++;
                }
            }
        }
    }
}
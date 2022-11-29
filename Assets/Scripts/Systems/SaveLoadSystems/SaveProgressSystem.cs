using Leopotam.EcsLite;
using Services;

namespace Systems.SaveLoadSystems
{
  public class SaveProgressSystem : IEcsInitSystem, IEcsRunSystem
  {
    private EcsWorld _world;
    private EcsFilter _filter;
    private SharedData _sharedData;
    private EcsPool<IsSaveProgressRequestComponent> _isSaveProgressPool;

    private SaveData _saveData;
    private int _currentLevel;


    public void Init(IEcsSystems systems)
    {
      _world = systems.GetWorld();
      _sharedData = systems.GetShared<SharedData>();
      _filter = _world.Filter<IsSaveProgressRequestComponent>().End();
      _isSaveProgressPool = _world.GetPool<IsSaveProgressRequestComponent>();


      _saveData = SaveService.Load();
      _currentLevel = _sharedData.GetMainData.LoadLevelNum;
    }

    public void Run(IEcsSystems systems)
    {
      foreach (var request in _filter)
      {
        UpdateSaveData();
        _isSaveProgressPool.Del(request);
      }
    }

    private void UpdateSaveData()
    {
      _saveData.Levels[_currentLevel].Available = Availability.Passed;
      if (NotLast())
        _saveData.Levels[NextLevel()].Available = Availability.Available;
      SaveService.Save(_saveData);
    }

    private bool NotLast() => 
      _sharedData.GetMainData.Levels.Data[NextLevel()] != null;

    private int NextLevel() => 
      _currentLevel + 1;
  }
}
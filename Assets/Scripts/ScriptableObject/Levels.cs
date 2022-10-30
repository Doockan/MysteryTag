using UnityEngine;

namespace MysteryTag
{
    [CreateAssetMenu(fileName = nameof(Levels))]
    public class Levels : ScriptableObject
    {
        [SerializeField] private LevelData[] _levels;

        public LevelData[] Data => _levels;
    }
}
using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = nameof(Levels))]
    public class Levels : UnityEngine.ScriptableObject
    {
        [SerializeField] private LevelData[] _levels;

        public LevelData[] Data => _levels;
    }
}
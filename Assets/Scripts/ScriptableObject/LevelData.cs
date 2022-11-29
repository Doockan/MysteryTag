using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = nameof(LevelData))]
    public class LevelData : UnityEngine.ScriptableObject
    {
        [SerializeField] private float _asteroidsSpeed;
        [SerializeField] private float _asteroidsRespawnSpeed;
        [SerializeField] private int _smallAsteroids;
        [SerializeField] private int _midlAsteroids;
        [SerializeField] private int _bigAsteroids;

        public float AsteroidsSpeed => _asteroidsSpeed;
        public float AsteroidsRespawnSpeed => _asteroidsRespawnSpeed;
        public int SmallAsteroids => _smallAsteroids;
        public int MidlAsteroids => _midlAsteroids;
        public int BigAsteroids => _bigAsteroids;
    }
}
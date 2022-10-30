using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MysteryTag
{
    [CreateAssetMenu(fileName = nameof(MainData))]
    public class MainData : ScriptableObject
    {
        [SerializeField] private float _shipSpeed;
        [SerializeField] private float _shipAttackSpeed;
        [SerializeField] private int _playerMaxHealth;
        [SerializeField] private int _projecttileSpeed;
        [SerializeField] private Levels _levels;

        public float GetShipSpeed => _shipSpeed;
        public float GetShipAttackSpeed => _shipAttackSpeed;
        public int GetMaxPlayerHealth => _playerMaxHealth;
        public int GetProjecttileSpeed => _projecttileSpeed;
        public int LoadLevelNum;
        public Levels Levels => _levels;
    }
}
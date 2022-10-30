using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MysteryTag
{
    public class MainHUDView : BaseView
    {
        [SerializeField] private TMP_Text _health;
        [SerializeField] private TMP_Text _score;
        [SerializeField] private GameObject _pausedWindow;
        [SerializeField] private GameObject _gameOverWindow;
        [SerializeField] private Button _menuButton;

        public TMP_Text Health => _health;
        public TMP_Text Score => _score;
        public GameObject PausedWindow => _pausedWindow;
        public GameObject GameOverWindow => _gameOverWindow;
        public Button MenuButton => _menuButton;
    }
}
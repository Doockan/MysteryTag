using TMPro;
using UnityEngine;

namespace MysteryTag
{
    public class LevelView : BaseView
    {
        [SerializeField] private TMP_Text _levelNumber;
        [SerializeField] private TMP_Text _levelAvailability;

        public TMP_Text LevelNumber => _levelNumber;
        public TMP_Text LevelAvailability => _levelAvailability;
    }
}
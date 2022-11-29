using UnityEngine;

namespace Views
{
    public class MainMenuView : BaseView
    {
        [SerializeField] private RectTransform _rectTransform;

        public RectTransform RectTransform => _rectTransform;
    }
}
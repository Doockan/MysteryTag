using UnityEngine;
using UnityEngine.UI;
using Leopotam.EcsLite;


namespace MysteryTag
{
    public sealed class ButtonView : BaseView
    {
        [SerializeField] private Button _button;
        private EcsPool<IsClickEventComponent> _clickPool;


        public override void Init(EcsWorld world, int entity)
        {
            base.Init(world, entity);
            _clickPool = World.GetPool<IsClickEventComponent>();
        }

        private void AddClickComponent()
        {
            if (!_clickPool.Has(Entity)) _clickPool.Add(Entity);
        }

        private void OnEnable() => _button?.onClick.AddListener(AddClickComponent);

        private void OnDisable() => _button?.onClick.RemoveAllListeners();
                       
        public void SetInteractable(bool mode) => _button.interactable = mode;
    }
}
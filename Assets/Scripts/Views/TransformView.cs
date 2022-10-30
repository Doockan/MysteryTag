using UnityEngine;

namespace MysteryTag
{
    public sealed class TransformView : BaseView
    {
        [SerializeField] private Transform _transform;

        public Transform Transform => _transform;
        public int GetEntity => Entity;

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
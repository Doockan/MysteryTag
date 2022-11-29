using UnityEngine;

namespace Views
{
    public sealed class FirePointView : BaseView
    {
        [SerializeField] private Transform _firePoint;

        public Transform Transform => _firePoint;
    }
}
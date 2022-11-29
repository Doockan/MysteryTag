using UnityEngine;

namespace Views
{
    public class RigidbodyView : BaseView
    {
        [SerializeField] private Rigidbody _rigidbody;

        public Rigidbody Rigidbody => _rigidbody;
    }
}
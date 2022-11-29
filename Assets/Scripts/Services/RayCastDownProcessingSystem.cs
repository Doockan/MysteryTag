using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.EventSystems;
using Views;

namespace Services
{
    public sealed class RayCastDownProcessingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private PointerEventData _pointerData;
        private List<RaycastResult> _resultsData;
        private ObjectView _objectView;


        public void Init(IEcsSystems systems)
        {
            _pointerData = new PointerEventData(EventSystem.current);
            _resultsData = new List<RaycastResult>();
        }

        public void Run(IEcsSystems systems)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TrackDownGraphicsRayCast();
                TrackDownPhysicsRayCast();
            }
        }

        private void TrackDownGraphicsRayCast()
        {
            _pointerData.position = Input.mousePosition;
            EventSystem.current?.RaycastAll(_pointerData, _resultsData);

            foreach (var result in _resultsData)
            {
                if (result.gameObject.TryGetComponent<ObjectView>(out _objectView))
                    _objectView.MarkCastDown();
            }
        }

        private void TrackDownPhysicsRayCast()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics2D.RaycastAll(ray.origin, ray.direction);

            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent<ObjectView>(out _objectView))
                    _objectView.MarkCastDown();
            }
        }
    }
}
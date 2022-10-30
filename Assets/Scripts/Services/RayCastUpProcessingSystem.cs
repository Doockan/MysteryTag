using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MysteryTag
{
    public sealed class RayCastUpProcessingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private PointerEventData _pointerData;
        private List<RaycastResult> _resultsData;
        private ObjectView _objectView;


        public void Init(IEcsSystems systems)
        {
            _pointerData = new PointerEventData(EventSystem.current);
            _resultsData = new List<RaycastResult>();
        }

        private void TrackDownGraphicsRayCast()
        {
            _pointerData.position = Input.mousePosition;
            EventSystem.current?.RaycastAll(_pointerData, _resultsData);

            for (int i = 0; i < _resultsData.Count; i++)
            {
                if (_resultsData[i].gameObject.TryGetComponent<ObjectView>(out _objectView)) 
                    _objectView.MarkCastUp();
            }
        }

        private void TrackDownPhysicsRayCast()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics2D.RaycastAll(ray.origin, ray.direction);

            for (var i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.TryGetComponent<ObjectView>(out _objectView))
                    _objectView.MarkCastUp();
            }
        }

        public void Run(IEcsSystems systems)
        {
            if (Input.GetMouseButtonUp(0))
            {
                TrackDownGraphicsRayCast();
                TrackDownPhysicsRayCast();
            }
        }
    }
}
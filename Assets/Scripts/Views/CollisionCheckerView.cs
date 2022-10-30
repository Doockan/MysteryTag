using UnityEngine;

namespace MysteryTag
{
    public class CollisionCheckerView : BaseView
    {
        private void OnCollisionEnter(Collision collision)
        {
            var hitPool = World.GetPool<HitComponent>();
            ref var hitComponent = ref hitPool.Add(Entity);
            hitComponent.other = collision.gameObject;
        }
    }
}
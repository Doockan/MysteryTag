using Components;
using Leopotam.EcsLite;

namespace Views
{
    public sealed class ObjectView : BaseView
    {
        private EcsPool<IsCastDownEventComponent> _castDownPool;
        private EcsPool<IsCastUpEventComponent> _castUpPool;


        public override void Init(EcsWorld world, int entity)
        {
            base.Init(world, entity);
            _castDownPool = World.GetPool<IsCastDownEventComponent>();
            _castUpPool = World.GetPool<IsCastUpEventComponent>();
        }

        public void MarkCastDown()
        {
            if (!_castDownPool.Has(Entity)) _castDownPool.Add(Entity);
        }

        public void MarkCastUp()
        {
            if (!_castUpPool.Has(Entity)) _castUpPool.Add(Entity);
        }
    }
}
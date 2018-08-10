using UnityEngine;

namespace Modules
{
    public class Effectable : Module
    {
        protected Effectable(GameObject self) : base(self)
        {
        }
        public void ApplyEffect(GameObject source, IEffect fx)
        {
//            fx.Initialize(source,Self);
        }
        public void RemoveEffect(IEffect fx)
        {
            
//            fx.Terminate();
            
        }

    }

    public class Effect : IEffect
    {
        
        protected GameObject Source { get; private set; }
        protected GameObject Target { get; private set; }
        public virtual void Initialize(GameObject source, GameObject target)
        {
            Target = target;
        }

        public virtual void PreTick()
        {
        }

        public virtual void Tick()
        {
        }

        public virtual void PostTick()
        {
        }

        public virtual bool ShouldTerminate
        {
            get { return true; }
        }
        public virtual void Terminate()
        {
        }
    }
    public interface IEffect
    {
        void Initialize(GameObject source, GameObject target);
        void PreTick();
        void Tick();
        void PostTick();
        void Terminate();
        bool ShouldTerminate { get; }
    }
}
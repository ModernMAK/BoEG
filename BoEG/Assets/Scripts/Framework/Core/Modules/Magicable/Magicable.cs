using System;
using Framework.Types;
using Framework.Utility;

namespace Framework.Core.Modules
{
    public class Magicable : Statable, IMagicable
    {
        public Magicable(IMagicableData data) : this(data.MagicCapacity,data.MagicGeneration)
        {
        }
        public Magicable(float capacity, float generation) : base(capacity,generation)
        {
        }

        public virtual float Mana
        {
            get => Value;
            protected set => Value = value;
        }

        public virtual float ManaPercentage
        {
            get => Normal;
            protected set => Normal = value;
        }

        public virtual float ManaCapacity
        {
            get => Capacity;
            protected set => Capacity = value;
        }

        public virtual float ManaGeneration
        {
            get => Generation;
            protected set => Generation = value;
        }

        public void ModifyMana(float change)
        {
            var args = new MagicableEventArgs(change);
            OnModifying(args);
            Mana += change;
            OnModified(args);
        }

        public void SetMana(float mana)
        {
            //Current + Change = Desired
            //Therefore
            //Desired - Current = Change
            float change = mana - Mana;
            ModifyMana(change);
        }


        protected virtual void OnModified(MagicableEventArgs e)
        {
            Modified?.Invoke(this, e);
        }

        protected virtual void OnModifying(MagicableEventArgs e)
        {
            Modifying?.Invoke(this, e);
        }

        public event EventHandler<MagicableEventArgs> Modified;
        public event EventHandler<MagicableEventArgs> Modifying;
    }
}
using System;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

namespace DeleteMe
{
    public interface IMagicable
    {
        
        float Mana { get; }
        float ManaPercentage { get; }
        float ManaCapacity { get; }
        float ManaGeneration { get; }
        void ModifyMana(float change);
        void SetMana(float mana);
        event EventHandler<MagicableEventArgs> Modified;
        event EventHandler<MagicableEventArgs> Modifying;
    }

    
    public class Magicable : Statable, IMagicable
    {
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

    public class MagicableEventArgs : EventArgs
    {
        public MagicableEventArgs(float change)
        {
            Change = change;
        }

        public float Change { get; private set; }
    }

}
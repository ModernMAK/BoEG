using System;

namespace DeleteMe
{
    public class DamageTarget : IDamageTarget
    {
        public virtual void TakeDamage(Damage damage)
        {
            var args = new DamageEventArgs(damage);
            OnDamaging(args);
            Callback(damage);
            OnDamaged(args);            
        }
        
        protected Action<Damage> Callback { get; set; }


        protected virtual void OnDamaged(DamageEventArgs e)
        {
            Damaged?.Invoke(this, e);
        }

        protected virtual void OnDamaging(DamageEventArgs e)
        {
            Damaging?.Invoke(this, e);
        }
        
        public event EventHandler<DamageEventArgs> Damaged;
        public event EventHandler<DamageEventArgs> Damaging;
    }
    public interface IDamageTarget
    {
        void TakeDamage(Damage damage);
        event EventHandler<DamageEventArgs> Damaged;
        event EventHandler<DamageEventArgs> Damaging;
    }
    public class DamageEventArgs : EventArgs
    {
        public DamageEventArgs(Damage damage)
        {
            Damage = damage;
        }

        public Damage Damage { get; private set; }
    }
    public struct Damage
    {
        public Damage(float value, DamageType type, params DamageModifiers[] modifiers)
        {
            if (value < 0)
                value = 0;//Dont allow less than 0 damage
            
            Modifiers = MergerModifiers(modifiers);
            Type = type;
            Value = value;
        }

        private Damage(Damage damage)
        {
            Value = damage.Value;
            Modifiers = damage.Modifiers;
            Type = damage.Type;
        }
        
        public float Value { get; private set; }
        public DamageModifiers Modifiers { get; private set; }
        
        public DamageType Type { get; private set; }

        public Damage SetValue(float value)
        {
            if (value < 0f)
                value = 0f;
            return new Damage(this)
            {                
                Value = value,
            };
        }
        public Damage ModifyValue(float change)
        {
            return SetValue(Value + change);
        }
        public Damage SetType(DamageType type)
        {
            return new Damage(this)
            {
                Type = type
            };
        }
        public Damage SetModifiers(params DamageModifiers[] modifiers)
        {
            return new Damage(this)
            {
                Modifiers = MergerModifiers(modifiers)
            };
        }
        public Damage AddModifiers(params DamageModifiers[] modifiers)
        {
            return new Damage(this)
            {
                Modifiers = Modifiers | MergerModifiers(modifiers)
            };
        }
        public Damage RemoveModifiers(params DamageModifiers[] modifiers)
        {
            return new Damage(this)
            {
                Modifiers = Modifiers & ~MergerModifiers(modifiers)
            };
        }

        private static DamageModifiers MergerModifiers(DamageModifiers[] modifiers)
        {
            
            var modifier = (DamageModifiers)0;
            foreach (var mod in modifiers)
                modifier |= mod;
            return modifier;
        }
        public Damage Duplicate()
        {
            return new Damage(this);
        }
    }
    [Flags]
    public enum DamageModifiers
    {
        None = 0,
        Attack,
        Ability
    }

    public enum DamageType
    {
        Physical,
        Magical,
        Pure
    }
}
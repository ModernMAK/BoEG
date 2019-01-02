using UnityEngine;

namespace Framework.Types
{
    public struct Damage
    {
        public Damage(float damageValue, DamageType damageType, GameObject source) : this()
        {
            Value = damageValue;
            Type = damageType;
            Source = source;
        }

        public Damage(Damage damage) : this(damage.Value, damage.Type, damage.Source)
        {
        }

        public GameObject Source { get; private set; }
        public float Value { get; private set; }
        public DamageType Type { get; private set; }

        public Damage SetSource(GameObject source)
        {
            return new Damage(this)
            {
                Source = source
            };
        }

        public Damage SetValue(float value)
        {
            return new Damage(this)
            {
                Value = value
            };
        }

        public Damage SetType(DamageType type)
        {
            return new Damage(this)
            {
                Type = type
            };
        }
    }
}
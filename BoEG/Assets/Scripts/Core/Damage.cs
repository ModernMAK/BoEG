using UnityEngine;
using Util;

namespace Core
{
    public struct Damage
    {
        public Damage(float damageValue, DamageType damageType, GameObject source) : this()
        {
            Value = damageValue;
            Type = damageType;
            Source = source;
        }
        public GameObject Source { get; private set; }
        public float Value { get; private set; }
        public DamageType Type { get; private set; }  
    }
}
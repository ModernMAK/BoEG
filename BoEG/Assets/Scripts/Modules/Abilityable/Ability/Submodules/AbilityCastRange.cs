using System;
using UnityEngine;

namespace Modules.Abilityable.Ability
{
    [Serializable]
    public abstract class AbilityCastRange : Ability.AbilitySubmodule , IAbilityCastRange
    {
        public abstract float CastRange { get; }

        public bool InCastRange(Vector3 point)
        {
            var range = CastRange;
            return ((point - Self.transform.position).sqrMagnitude <= (range * range));
        }
        [Serializable]
        public class Flat : AbilityCastRange
        {
            [SerializeField] private float _castRange;
            public override float CastRange
            {
                get { return _castRange; }
            }
        }
        [Serializable]
        public class Scalar : AbilityCastRange
        {
            [SerializeField] private float[] _castRange;
            public override float CastRange
            {
                get { return Ability.GetLeveledData(_castRange); }
            }
        }
    }
    public interface IAbilityCastRange
    {
        float CastRange { get; }

        bool InCastRange(Vector3 point);
    }
}
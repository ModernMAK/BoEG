using System;
using UnityEngine;

namespace Entity.Abilities.ScarTheLastHunter
{
    public class BladeDash : Ability
    {
        [SerializeField] private BladeDashData[] _dashData = new BladeDashData[4];
    }

    [Serializable]
    public struct BladeDashData
    {
        public float Damage;
        public float Distance;
    }
}
using System;
using Framework.Core.Modules;
using UnityEngine;

namespace Entity.Abilities.FlameWitch
{


    public class CooldownHelper
    {
        public float Elapsed { get; set; }
        public float Cooldown { get; set; }

        public float Normal => Mathf.Clamp01(Elapsed / Cooldown);
        public bool Done => Cooldown <= Elapsed;

        public void Reset()
        {
            Elapsed = 0f;
        }

        public void Advance(float delta)
        {
            Elapsed += delta;
        }
    }
}
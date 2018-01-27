using System.Collections.Generic;
using Trigger;
using UnityEngine;

namespace Entity
{
    public class AreaOfEffectStrategy : TriggerStrategy
    {
        public AreaOfEffectStrategy()
        {
            Affected = new List<GameObject>();
        }

        public List<GameObject> Affected { get; private set; }

        public override bool ShouldEnter(GameObject go)
        {
            return !Affected.Contains(go);
        }

        public override void Enter(GameObject go)
        {
            Affected.Add(go);
        }


        public override bool ShouldExit(GameObject go)
        {
            return Affected.Contains(go);
        }

        public override void Exit(GameObject go)
        {
            Affected.Remove(go);
        }
    }
}
using UnityEngine;

namespace Entity.Abilities.ScarTheLastHunter
{
    public class PerceptionStrategy : AreaOfEffectStrategy
    {
        public PerceptionStrategy(GameObject owner)
        {
            Owner = owner;
        }

        public GameObject Owner { get; private set; }

        public Teamable Teamable
        {
            get { return Owner.GetComponent<Teamable>(); }
        }

        public override bool ShouldEnter(GameObject go)
        {
            return !Affected.Contains(go) && !IsAlly(go);
        }

        public override bool ShouldExit(GameObject go)
        {
            return Affected.Contains(go) && IsAlly(go);
        }

        private bool IsAlly(GameObject go)
        {
            var oTeamable = go.GetComponent<Teamable>();
            return Teamable.SafeIsAlly(oTeamable, true);
        }
    }
}
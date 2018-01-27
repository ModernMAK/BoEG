using Trigger;
using UnityEngine;

namespace Entity.Abilities.ScarTheLastHunter
{
    [RequireComponent(typeof(Buffable.Buffable))]
    public class Perception : Ability
    {
        private PerceptionBuff _perceptionBuff;
        private AreaOfEffectStrategy _percStrategy;
        private SphereTrigger _percTrigger;

        public Visionable Visionable
        {
            get { return GetComponent<Visionable>(); }
        }

        public Buffable.Buffable Buffable
        {
            get { return GetComponent<Buffable.Buffable>(); }
        }

        public bool IsWatched
        {
            get { return _percStrategy.Affected.Count > 0; }
        }


        protected override void Awake()
        {
            base.Awake();
            _perceptionBuff = new PerceptionBuff();
            _percStrategy = new PerceptionStrategy(gameObject);
            _percTrigger = this.CreateTrigger<SphereTrigger>(_percStrategy, "Perception");
        }

        protected override void Update()
        {
            base.Update();
            _percTrigger.Radius = Visionable.NormVisRange;

            if (Buffable.HasBuff(_perceptionBuff) && !IsWatched)
                Buffable.RemoveBuff(_perceptionBuff);
        }
    }

    public class PerceptionBuff : IBuff
    {
    }
}
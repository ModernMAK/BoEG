using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Modules.Healthable;
using Modules.Teamable;
using Triggers;
using UnityEngine;

namespace Entity.Abilities
{
    public abstract class Decorator<T> where T : Decorator<T>
    {
        protected T Child { get; private set; }

        protected Decorator(T child)
        {
            Child = child;
        }

        public U SetChild<U>(U child) where U : T
        {
            Child = child;
            return child;
        }


        public T Duplicate()
        {
            var dup = DuplicateSelf();
            if (Child != null)
                dup.SetChild(Child.Duplicate());
            return dup;
        }

        protected abstract T DuplicateSelf();
    }

    public abstract class Effect : Decorator<Effect>
    {
        protected Effect(Effect child = null) : base(child)
        {
        }

        public void ApplyEffect(GameObject go)
        {
            InternalApplyEffect(go);
            if (Child != null)
                Child.InternalApplyEffect(go);
        }

        protected abstract void InternalApplyEffect(GameObject go);
    }


    public class DamageEffect : Effect
    {
        public DamageEffect(Damage damage, Effect child = null) : base(child)
        {
            _damage = damage;
        }

        private Damage _damage;

        protected override Effect DuplicateSelf()
        {
            return new DamageEffect(_damage);
        }

        public DamageEffect SetDamage(Damage damage)
        {
            _damage = damage;
            return this;
        }

        protected override void InternalApplyEffect(GameObject go)
        {
            var healthable = go.GetComponent<IHealthable>();
            if (healthable != null)
                healthable.TakeDamage(_damage);
        }
    }

    public abstract class Affector : Decorator<Affector>
    {
        protected Affector(Affector child) : base(child)
        {
        }
        public IEnumerable<GameObject> Affects(IEnumerable<GameObject> gos)
        {
            gos = gos.Where(InternalAffects);
            if (Child != null)
                gos = Child.Affects(gos);
            return gos;
        }

        protected abstract bool InternalAffects(GameObject go);
    }
    
    public class TeamAffector : Affector
    {
        public TeamAffector(ITeamable team, Func<ITeamable, ITeamable, bool> validationFunc = null,
            bool affectNull = false, Affector child = null) : base(child)
        {
            _team = team;
            _func = validationFunc;
            _affectNull = affectNull;
        }

        public class Enemy : TeamAffector
        {
            public Enemy(ITeamable team, bool affectNull = false, Affector child = null) : base(team, AffectEnemies,
                affectNull, child)
            {
            }

            public static bool AffectEnemies(ITeamable self, ITeamable other)
            {
                return self.Team != other.Team;
            }
        }

        public class Allies : TeamAffector
        {
            public Allies(ITeamable team, bool affectNull = false, Affector child = null) : base(team, AffectAllies,
                affectNull, child)
            {
            }

            public static bool AffectAllies(ITeamable self, ITeamable other)
            {
                return self.Team == other.Team;
            }
        }


        private readonly ITeamable _team;
        private readonly Func<ITeamable, ITeamable, bool> _func;
        private readonly bool _affectNull;

        protected override Affector DuplicateSelf()
        {
            return new TeamAffector(_team, _func,_affectNull);
        }

        protected override bool InternalAffects(GameObject go)
        {
            var oTeam = go.GetComponent<ITeamable>();
            if (_team == null || oTeam == null) return _affectNull;
            return _func(_team, oTeam);
        }
    }

    public abstract class Selector : Decorator<Selector>
    {
        
        protected Selector(Selector child) : base(child)
        {
        }
        public IEnumerable<GameObject> Select()
        {
            var gos = InternalSelect();
            if (Child != null)
                gos = gos.Concat(Child.Select());
            return gos;
        }

        protected abstract IEnumerable<GameObject> InternalSelect();
    }

    public class TriggerSelector : Selector
    {
        public TriggerSelector(Trigger trigger, Selector child = null) : base(child)
        {
            _trigger = trigger;
        }

        private Trigger _trigger;

        public TriggerSelector SetTrigger(Trigger trigger)
        {
            _trigger = trigger;
            return this;
        }
        protected override IEnumerable<GameObject> InternalSelect()
        {
            return _trigger.Colliders;
        }
        

        protected override Selector DuplicateSelf()
        {
            return new TriggerSelector(_trigger);
        }
    }

    public static class EffectAffectSelectUtil
    {
        public static void Apply(Selector selector, Affector affector, Effect effect)
        {
            foreach (var go in affector.Affects(selector.Select()))
            {
                effect.ApplyEffect(go);
            }

            
        }
        public static Action<Selector,Effect> Apply(this Action<Selector,Affector,Effect> func, Affector affector)
        {
            return (s, e) => func(s, affector, e);
        }
        public static Action<Selector,Affector> Apply(this Action<Selector,Affector,Effect> func, Effect effect)
        {
            return (s, a) => func(s, a, effect);
        }
        public static Action<Affector,Effect> Apply(this Action<Selector,Affector,Effect> func, Selector selector)
        {
            return (a, e) => func(selector, a, e);
        }
        
        public static Action<Affector> Apply(this Action<Selector,Affector> func, Selector selector)
        {
            return (a) => func(selector, a);
        }
        public static Action<Selector> Apply(this Action<Selector,Affector> func, Affector affector)
        {
            return (s) => func(s, affector);
        }
        public static Action<Effect> Apply(this Action<Affector,Effect> func, Affector affector)
        {
            return (e) => func(affector, e);
        }
        public static Action<Affector> Apply(this Action<Affector,Effect> func, Effect effect)
        {
            return (a) => func(a, effect);
        }
        public static Action<Selector> Apply(this Action<Selector,Effect> func, Effect effect)
        {
            return (s) => func(s, effect);
        }
        public static Action<Effect> Apply(this Action<Selector,Effect> func, Selector selector)
        {
            return (e) => func(selector, e);
        }
        public static Action Apply(this Action<Selector> func, Selector selector)
        {
            return () => func(selector);
        }
        public static Action Apply(this Action<Affector> func, Affector affector)
        {
            return () => func(affector);
        }
        public static Action Apply(this Action<Effect> func, Effect effect)
        {
            return () => func(effect);
        }
    }

//Ticks are affectors
//Affectors apply effects
//    public abstract class Affector
//    {
//        protected virtual void Initialize()
//        {
//        }
//
//        protected virtual void Terminate()
//        {
//        }
//
//        private Affector _child;
//
//        protected Affector(Affector child = null)
//        {
//            _child = child;
//        }
//
//        public T SetChild<T>(T affector) where T : Affector
//        {
//            _child = affector;
//            return affector;
//        }
//
//        public void Step(float deltaStep)
//        {
//            StepSelf(deltaStep);
//            if (_child != null)
//            {
//                _child.StepSelf(deltaStep);
//            }
//        }
//
//
//        protected virtual void StepSelf(float deltaStep)
//        {
//        }
//
//        //Applies the affect to the instances
//        public abstract void InternalApplyEffect();
//
//        //Applies the affect to the instances
//        public void InternalApplyEffect(IEnumerable<GameObject> gos)
//        {
//            foreach (var go in gos)
//            {
//                if (InternalApplyEffect(go))
//                    if (_child != null)
//                        _child.InternalApplyEffect(go);
//            }
//        }
//
//        //Applies the affect to the instance
//        protected abstract bool InternalApplyEffect(GameObject go);
//    }
//
////        public class TickAffector : Affector
////        {   
////        }
//
//
//    public class EffectAffector : Affector
//    {
//        private readonly Effect _effect;
//
//        public EffectAffector(Effect effect) : base()
//        {
//            _effect = effect;
//        }
//
//        protected override bool InternalApplyEffect(GameObject go)
//        {
//            if (_effect == null) return false;
//            _effect.InternalApplyEffect(go);
//            return true;
//        }
//
//        public override void InternalApplyEffect()
//        {
//            return;//Effect Affector Cannot Discover Targets         
//        }
//    }
//
//    public class TriggerAffector : Affector
//    {
//        public TriggerAffector(Trigger trigger, Affector child = null) : base(child)
//        {
//            _trigger = trigger;
//        }
//
//        private readonly Trigger _trigger;
//
//
//        public override void InternalApplyEffect()
//        {
//            InternalApplyEffect(_trigger.Colliders);
//        }
//        protected override bool InternalApplyEffect(GameObject go)
//        {
//            return (_trigger.Colliders.Contains(go));
//        }
//    }
//
//    public class TickAffector : Affector
//    {
//        public struct TickData
//        {
//            private float _tickInterval;
//            private float _tickDuration;
//
//            public float TickInterval
//            {
//                get { return _tickInterval; }
//            }
//
//            public float TickDuration
//            {
//                get { return _tickDuration; }
//            }
//
//            public TickData Duplicate()
//            {
//                return new TickData()
//                {
//                    _tickInterval = TickInterval,
//                    _tickDuration = TickDuration
//                };
//            }
//        }
//
//        public class TickInstance
//        {
//            public TickInstance(TickData data)
//            {
//                _tickData = data;
//                _timeElapsed = 0f;
//                _ticksPerformed = 0;
//            }
//
//            private readonly TickData _tickData;
//            private float _timeElapsed;
//            private int _ticksPerformed;
//
//            public void AdvanceTime(float deltaTime)
//            {
//                _timeElapsed += deltaTime;
//            }
//
//            public int TicksToPerform
//            {
//                get
//                {
//                    if (_timeElapsed > _tickData.TickDuration)
//                    {
//                        return Mathf.FloorToInt(_tickData.TickDuration / _tickData.TickInterval) - _ticksPerformed;
//                    }
//                    else
//                    {
//                        var performedTime = _tickData.TickInterval * _ticksPerformed;
//                        var remainingTime = performedTime - _timeElapsed;
//                        var ticksToPerform = remainingTime / _tickData.TickInterval;
//                        return Mathf.FloorToInt(ticksToPerform);
//                    }
//                }
//            }
//
//            public void AdvanceTicks()
//            {
//                _ticksPerformed += TicksToPerform;
//            }
//        }
//
//        public TickAffector(TickData tick, Affector child = null) : base(child)
//        {
//            _tickData = new TickInstance(tick);
//        }
//
//        private readonly TickInstance _tickData;
//
//
//        protected override void StepSelf(float deltaStep)
//        {
//            
//        }
//
//        protected override bool InternalApplyEffect(GameObject go)
//        {
//            for(var i = 0; i )
//            return (.Colliders.Contains(go));
//        }
//    }
}
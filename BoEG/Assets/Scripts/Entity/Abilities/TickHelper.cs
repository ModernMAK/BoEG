using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Abilities
{

    public class TickActionContainer<T> where T : TickAction
    {
        private readonly List<T> _internalContainer;

        public TickActionContainer()
        {
            _internalContainer = new List<T>();
        }

        public void Add(T instance)
        {
            _internalContainer.Add(instance);    
        }

        public void Tick(float deltaTick)
        {
            for (var i = 0; i < _internalContainer.Count; i++)
            {
                var inst = _internalContainer[i];
                if (inst.DoneTicking)
                {
                    inst.Terminate();
                    _internalContainer.RemoveAt(i);
                    i--;
                }
                else
                {
                    inst.Tick(deltaTick);
                }
            }
        }

        public void Terminate()
        {
            foreach (var inst in _internalContainer)
            {
                inst.Terminate();
            }
            _internalContainer.Clear();
        }
            
            
    }
    public sealed class TickActionDelegate : TickAction
    {
        public TickActionDelegate(int required, float interval, Action logic) : base(required, interval)
        {
            Delegate = logic;
        }

        private Action Delegate { get; set; }

        protected override void Logic()
        {
            if (Delegate != null)
                Delegate();
        }
    }

    public abstract class TickAction
    {
        protected TickAction(int required, float duration)
        {
            TicksRequired = required;
            TickDuration = duration;
            TicksPerformed = 0;
            ElapsedTime = 0f;
        }


        protected float TickDuration { get; private set; }
        protected int TicksRequired { get; private set; }
        protected int TicksPerformed { get; private set; }

        protected float TickInterval
        {
            get { return TickDuration / TicksRequired; }
        }

        protected float ElapsedTime { get; private set; }

        protected int TicksToPerform
        {
            get
            {
                var ticksLeft = TicksRequired - TicksPerformed;
                var allowedTicks = Mathf.FloorToInt(ElapsedTime / TickInterval);
                return Mathf.Min(ticksLeft, allowedTicks);
            }
        }

        public bool DoneTicking
        {
            get { return TicksRequired <= TicksPerformed; }
        }

        protected abstract void Logic();

        public void Tick(float time)
        {
            AdvanceTime(time);
            Logic();
            AdvanceTicks();
        }

        public virtual void Terminate()
        {
            
        }

        private void AdvanceTime(float time)
        {
            ElapsedTime += time;
        }


        private void AdvanceTicks()
        {
            var temp = TicksToPerform;
            TicksPerformed += temp;
            ElapsedTime -= TickInterval * temp;
        }
    }
}
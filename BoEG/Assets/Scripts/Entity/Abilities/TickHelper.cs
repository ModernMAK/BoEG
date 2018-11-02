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
        public TickActionDelegate(TickData data, Action logic) : base(data)
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
        protected TickAction(TickData data)
        {
            Data = data;
            Performed = 0;
            ElapsedTime = 0f;
        }

        protected TickData Data { get; private set; }

        protected float Duration
        {
            get { return Data.Duration; }
        }

        protected float Interval
        {
            get { return Data.Interval; }
        }

        protected int Performed { get; private set; }


        protected float ElapsedTime { get; private set; }

        protected int TicksRemaining
        {
            get { return Mathf.FloorToInt(Duration - (Interval * Performed) / Interval); }
        }

        protected int TicksToPerform
        {
            get
            {
                var performing = Mathf.FloorToInt(ElapsedTime / Interval);
                return Mathf.Min(performing, TicksRemaining);
            }
        }

        public bool DoneTicking
        {
            get { return TicksRemaining <= 0; }
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
            Performed += temp;
            ElapsedTime -= Interval * temp;
        }
    }
}
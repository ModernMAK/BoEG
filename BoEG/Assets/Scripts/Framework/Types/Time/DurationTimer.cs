using System;
using MobaGame.Framework.Core;

namespace MobaGame.Framework.Types
{
    public class DurationTimer : IView
    {
        public DurationTimer(float duration, bool startDone = false) 
        {
            Duration = duration;
            ElapsedTime = startDone ? Duration : 0f;
        }

        private float _elapsedTime;
        public float ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                _elapsedTime = value;
                OnChanged();
            }
        }

        private float _duration;
        public float Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnChanged();
                
            } 
        }
        public float RemainingTime => Duration - ElapsedTime;
        public bool Done => RemainingTime <= 0f;


        public void Reset()
            =>ElapsedTime = 0;
        

        public void SetDone()=>
            ElapsedTime = Duration;
        

        public bool AdvanceTime(float deltaTime)
        {
            ElapsedTime += deltaTime;
            return Done;
        }
        

        public bool AdvanceTimeIfNotDone(float deltaTime) => Done || AdvanceTime(deltaTime);
        public event EventHandler Changed;
       
        private void OnChanged()=> Changed?.Invoke(this,EventArgs.Empty);
    }
}
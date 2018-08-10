using System.Collections.Generic;
using Modules;
using UnityEngine;

namespace Core.OrderSystem
{
    public class JobSystem : Module, IJobSystem
    {
        public JobSystem(GameObject self) : base(self)
        {
            _jobQueue = new Queue<IJob>();
            _activeJob = null;
        }

        private readonly Queue<IJob> _jobQueue;
        private IJob _activeJob;

        public override void Initialize()
        {
        }


        public override void PreTick(float deltaTick)
        {
            if (_activeJob != null)
            {
                _activeJob.PreTick(deltaTick);
            }
        }

        public override void Tick(float deltaTick)
        {
            if (_activeJob != null)
            {
                _activeJob.Tick(deltaTick);
            }
        }

        public override void PostTick(float deltaTick)
        {
            if (_activeJob != null)
            {
                _activeJob.PostTick(deltaTick);
                if (_activeJob.IsDone())
                {
                    _activeJob.Terminate();
                    _activeJob = null;
                }
            }

            if (_activeJob == null && _jobQueue.Count > 0)
            {
                _activeJob = _jobQueue.Dequeue();
                _activeJob.Initialize(Self);
            }
        }

        public override void PhysicsTick(float deltaTick)
        {
            if (_activeJob != null)
            {
                _activeJob.PhysicsTick(deltaTick);
            }
        }

        public void StopJobs()
        {
            if (_activeJob != null)
            {
                _activeJob.Terminate();
                _activeJob = null;
            }

            _jobQueue.Clear();
        }

        public void SetJob(IJob job)
        {
            StopJobs();
            AddJob(job);
        }

        public void AddJob(IJob job)
        {
            _jobQueue.Enqueue(job);
        }
    }
}
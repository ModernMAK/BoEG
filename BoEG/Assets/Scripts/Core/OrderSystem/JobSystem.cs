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


        public override void PreStep(float deltaStep)
        {
            if (_activeJob != null)
            {
                _activeJob.PreStep(deltaStep);
            }
        }

        public override void Step(float deltaTick)
        {
            if (_activeJob != null)
            {
                _activeJob.Step(deltaTick);
            }
        }

        public override void PostStep(float deltaTick)
        {
            if (_activeJob != null)
            {
                _activeJob.PostStep(deltaTick);
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

        public override void PhysicsStep(float deltaTick)
        {
            if (_activeJob != null)
            {
                _activeJob.PhysicsStep(deltaTick);
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
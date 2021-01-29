using MobaGame.Framework.Types;
using UnityEngine;
using UnityEngine.AI;

namespace MobaGame.Framework.Core.Modules
{
    public class Movable : ActorModule, IInitializable<IMovableData>, IMovable, IListener<IStepableEvent>
    {
        private NavMeshAgent _agent;
        private NavMeshObstacle _obstacle;

        public Movable(Actor actor, NavMeshAgent agent, NavMeshObstacle obstacle) : base(actor)
        {
            _agent = agent;
            _obstacle = obstacle;
        }

        public void Initialize(IMovableData data)
        {
            MoveSpeed = data.MoveSpeed;
            TurnSpeed = data.TurnSpeed;
            _obstacle.enabled = false;
        }

        public float MoveSpeed { get; private set; }
        public float TurnSpeed { get; private set; }


        private bool UnlockNav()
        {
            if (Anchored)
            {
                UnAnchor();
                return true;
            }

            return false;
        }

        private void RelockNav(bool wasAnchored)
        {
            if (wasAnchored && !Anchored)
                Anchor();
        }

        public void MoveTo(Vector3 destenation)
        {
            if (_agent.gameObject.activeInHierarchy)
            {
                var locked = UnlockNav();
                _agent.SetDestination(destenation);
                RelockNav(locked);
            }
        }

        public void WarpTo(Vector3 destenation)
        {
            if (_agent.gameObject.activeSelf)
            {
                var locked = UnlockNav();
                _agent.Warp(destenation);
                RelockNav(locked);
            }
        }

        public void Push(Vector3 direction)
        {
            if (_agent.gameObject.activeSelf)
            {
                var locked = UnlockNav();
                _agent.Move(direction);
                RelockNav(locked);
            }
        }

        public void Anchor()
        {
            //Disable first
            _agent.updatePosition = false;
            _agent.enabled = false;
            //Then enable
            _obstacle.enabled = true;
            _obstacle.carving = true;
        }

        public void UnAnchor()
        {
            //Disable first
            _obstacle.carving = false;
            _obstacle.enabled = false;
            //Then enable
            _agent.enabled = true;
            _agent.updatePosition = true;
        }

        public bool Anchored => _obstacle.enabled;

        public void StopMovement()
        {
            if (_agent.gameObject.activeSelf)
            {
                var locked = UnlockNav();
                _agent.isStopped = true;
                RelockNav(locked);
            }
        }

        public void StartMovement()
        {
            if (_agent.gameObject.activeSelf)
            {
                var locked = UnlockNav();
                _agent.isStopped = false;
                RelockNav(locked);
            }
        }

        public void CancelMovement()
        {
            if (_agent.gameObject.activeSelf)
            {
                var locked = UnlockNav();
                _agent.ResetPath();
                RelockNav(locked);
            }
        }

        public bool HasReachedDestination => (_agent.destination - _agent.nextPosition).sqrMagnitude <= Mathf.Epsilon;

        public void OnPreStep(float deltaTime)
        {
            _agent.speed = MoveSpeed;
            _agent.acceleration = MoveSpeed * 1000;
            _agent.angularSpeed = TurnSpeed;
        }


        public void Register(IStepableEvent source)
        {
            source.PreStep += OnPreStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= OnPreStep;
        }
    }
}
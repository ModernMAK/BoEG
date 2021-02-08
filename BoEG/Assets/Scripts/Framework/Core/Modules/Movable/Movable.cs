using MobaGame.Framework.Types;
using UnityEngine;
using UnityEngine.AI;

namespace MobaGame.Framework.Core.Modules
{
    public interface IMoveSpeedModifier : IModifier
    {
        FloatModifier MoveSpeed { get; }
    }
    public interface ITurnSpeedModifier : IModifier
    {
        FloatModifier TurnSpeed { get; }
    }
    public class Movable : ActorModule, IInitializable<IMovableData>, IMovable, IListener<IStepableEvent>,IListener<IModifiable>, IRespawnable
    {
        private NavMeshAgent _agent;
        private NavMeshObstacle _obstacle;

        public Movable(Actor actor, NavMeshAgent agent, NavMeshObstacle obstacle) : base(actor)
        {
            _agent = agent;
            _obstacle = obstacle;
            MoveSpeed = new ModifiedValueBoilerplate<IMoveSpeedModifier>(modifier=>modifier.MoveSpeed);
            TurnSpeed = new ModifiedValueBoilerplate<ITurnSpeedModifier>(modifiier=>modifiier.TurnSpeed);
        }

        public void Initialize(IMovableData data)
        {
            MoveSpeed.Value.Base = data.MoveSpeed;
            TurnSpeed.Value.Base = data.TurnSpeed;
            _obstacle.enabled = false;
        }

        public ModifiedValueBoilerplate<IMoveSpeedModifier> MoveSpeed { get; }
        public ModifiedValueBoilerplate<ITurnSpeedModifier> TurnSpeed { get; }

        IModifiedValue<float> IMovable.MoveSpeed => MoveSpeed.Value;
        IModifiedValue<float> IMovable.TurnSpeed => TurnSpeed.Value;


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
            _agent.speed = MoveSpeed.Value.Total;
            _agent.acceleration = _agent.speed * 1000;
            _agent.angularSpeed = TurnSpeed.Value.Total;
        }


        public void Register(IStepableEvent source)
        {
            source.PreStep += OnPreStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= OnPreStep;
        }

		public void Register(IModifiable source)
		{
            MoveSpeed.Register(source);
            TurnSpeed.Register(source);
        }

		public void Unregister(IModifiable source)
        {
            MoveSpeed.Unregister(source);
            TurnSpeed.Unregister(source);
        }

        public void Respawn()
        {
            UnAnchor();
            CancelMovement();
        }
    }
}
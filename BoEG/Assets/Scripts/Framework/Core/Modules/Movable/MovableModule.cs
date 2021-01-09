using System;
using MobaGame.Framework.Types;
using UnityEngine;
using UnityEngine.AI;

namespace MobaGame.Framework.Core.Modules
{
    [DisallowMultipleComponent]
    //We use stationary on navmesh obstacle, which means we dont need to use it here
    [RequireComponent(typeof(NavMeshObstacle))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovableModule : MonoBehaviour, IInitializable<IMovableData>, IMovable, IListener<IStepableEvent>
    {
        private Movable _movable;

        private void Awake()
        {
            var actor = GetComponent<Actor>();
            var agent = GetComponent<NavMeshAgent>();
            var obstacle = GetComponent<NavMeshObstacle>();
            _movable = new Movable(actor, agent, obstacle);
        }

        public void Initialize(IMovableData module) => _movable.Initialize(module);

        public float MoveSpeed => _movable.MoveSpeed;
        public float TurnSpeed => _movable.TurnSpeed;


        public void MoveTo(Vector3 destenation) => _movable.MoveTo(destenation);

        public void WarpTo(Vector3 destenation) => _movable.WarpTo(destenation);

        public void Push(Vector3 direction) => _movable.Push(direction);

        public void Anchor() => _movable.Anchor();

        public void UnAnchor() => _movable.UnAnchor();

        public bool Anchored => _movable.Anchored;

        public void StopMovement() => _movable.StopMovement();

        public void StartMovement() => _movable.StartMovement();

        public void CancelMovement() => _movable.CancelMovement();

        public bool HasReachedDestination => _movable.HasReachedDestination;
        public void Register(IStepableEvent source) => _movable.Register(source);


        public void Unregister(IStepableEvent source) => _movable.Unregister(source);
    }
}
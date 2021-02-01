using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Commands
{
    public class AttackMoveCommand : GameObjectCommand
    {
        private readonly IAggroable _aggroable;
        private readonly IAttackerable _attackerable;
        private readonly IMovable _movable;
        private readonly Vector3 _destenation;

        public AttackMoveCommand(GameObject entity, Vector3 destenation) : base(entity)
        {
            GetModule(out _aggroable);
            GetModule(out _attackerable);
            GetModule(out _movable);
            _destenation = destenation;
        }


        protected override void Step(float deltaTime)
        {
            if (_attackerable.HasTarget())
            {
                _movable.StopMovement();
                _movable.Anchor();
                if (!_attackerable.OnCooldown)
                {
                    var target = _attackerable.Targets[0];
                    _attackerable.Attack(target);
                }
            }
            //Aggroable is optional so we check for null
            else if (_aggroable != null && _aggroable.HasTarget()) 
            {
                _movable.UnAnchor();
                var target = _aggroable.Targets[0];
                var position = target.transform.position;
                _movable.MoveTo(position);
                _movable.StartMovement();
            }
            else
            {
                _movable.UnAnchor();
                _movable.StartMovement();
                _movable.MoveTo(_destenation);
            }
        }

        protected override void Start()
        {
            _movable.StopMovement();
            _movable.MoveTo(_destenation);
            _movable.StartMovement();
        }

        protected override void Stop()
        {
            _movable.StopMovement();
        }

        protected override bool IsDone()
        {
            return false;
        }
    }

    //TODO fix this
    public class AttackHoldCommand : GameObjectCommand
    {
        private readonly IAggroable _aggroable;
        private readonly IAttackerable _attackerable;

        public AttackHoldCommand(GameObject entity) : base(entity)
        {
            GetModule(out _attackerable);
            GetModule(out _aggroable);
        }

        protected IAttackerable Attackerable => _attackerable;
        protected IAggroable Aggroable => _aggroable;


        protected override void Step(float deltaTime)
        {
            if (Attackerable.OnCooldown)
            {
            }

            throw new NotImplementedException();
//            if (Attackerable.CanAttack)
//            {
//                if (Attackerable.HasAttackTarget())
//                    Attackerable.Attack(Attackerable.GetAttackTargets().First());
//            }
        }

        protected override void Start()
        {
            //Do Nothing
        }

        protected override void Stop()
        {
            //Do nothing
        }

        protected override bool IsDone()
        {
            return false;
        }
    }
}
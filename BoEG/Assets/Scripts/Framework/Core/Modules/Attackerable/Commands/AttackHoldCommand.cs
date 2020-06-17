using System;
using UnityEngine;

namespace Framework.Core.Modules.Commands
{
    public class AttackMoveCommand : EntityCommand
    {
        // private readonly IAggroable _aggroable;
        private readonly IAttackerable _attackerable;
        private readonly IMovable _movable;
        private readonly Vector3 _destenation;

        public AttackMoveCommand(GameObject entity, Vector3 destenation) : base(entity)
        {
            // GetComponent(out _aggroable);
            GetComponent(out _attackerable);
            GetComponent(out _movable);
            _destenation = destenation;
        }


        protected override void Step(float deltaTime)
        {
            if (_attackerable.HasAttackTarget())
            {
                _movable.StopMovement();
                if (_attackerable.IsAttackOnCooldown)
                {
                    _attackerable.Attack(_attackerable.GetAttackTarget(0));
                }
            }
            else
            {
                _movable.StartMovement();
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

    public class AttackHoldCommand : EntityCommand
    {
        private readonly IAggroable _aggroable;
        private readonly IAttackerable _attackerable;

        public AttackHoldCommand(GameObject entity) : base(entity)
        {
            GetComponent(out _attackerable);
            GetComponent(out _aggroable);
        }

        protected IAttackerable Attackerable => _attackerable;
        protected IAggroable Aggroable => _aggroable;


        protected override void Step(float deltaTime)
        {
            if (Attackerable.IsAttackOnCooldown)
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

//    public class AttackMoveCommand : EntityCommand
//    {
//        protected IAttackerable Attackerable { get; private set; }
//        protected IMovable Movable { get; private set; }
//
//        public AttackMoveCommand(GameObject entity) : base(entity)
//        {
//            Attackerable = GetComponent<IAttackerable>();
//            Movable = GetComponent<IMovable>();
//        }
//
//        private GameObject[] GetTargets()
//        {
//            var cols = Physics.OverlapSphere(Entity.transform.position, Attackerable.AttackRange);
//            var gos = new List<GameObject>(cols.Select((c) => c.gameObject).Where((g) => (g != Entity)));
//            gos.Sort((x, y) =>
//            {
//                Vector3 position = Entity.transform.position;
//                return (x.transform.position - position).sqrMagnitude.CompareTo(
//                    y.transform.position - (position));
//            });
//            return gos.ToArray();
//        }
//
//
//        protected override void Step(float delta)
//        {
//            base.Step(delta);
//            if (Attackerable.CanAttack)
//            {
//                var targets = GetTargets();
//                if (targets.Length > 0)
//                {
//                    Attackerable.Attack(targets[0]);
//                    Movable.StopMovement();
//                }                
//            }
//            Movable.StartMovement();
//            
//        }
//
//        protected override bool IsDone()
//        {
//            return mo;
//        }
//    }
}
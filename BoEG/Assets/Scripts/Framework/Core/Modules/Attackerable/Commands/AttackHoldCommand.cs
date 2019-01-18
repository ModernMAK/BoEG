using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework.Core.Modules.Commands
{
    public class AttackHoldCommand : EntityCommand
    {
        protected IAttackerable Attackerable { get; private set; }

        public AttackHoldCommand(GameObject entity) : base(entity)
        {
            Attackerable = GetComponent<IAttackerable>();
        }


        protected override void Step(float delta)
        {
            base.Step(delta);
            if (Attackerable.CanAttack)
            {
                if (Attackerable.HasAttackTarget())
                    Attackerable.Attack(Attackerable.GetAttackTargets().First());
            }
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
using Modules.Movable;
using UnityEngine;

namespace Core.OrderSystem.Order
{
    public class MoveToJob : Job
    {
        public MoveToJob(Vector3 destenation)
        {
            _destenation = destenation;
        }

        private IMovable _movable;
        private readonly Vector3 _destenation;

        public override void Initialize(GameObject go)
        {
            _movable = go.GetComponent<IMovable>();
            if (_movable == null) return;
            _movable.MoveTo(_destenation);
        }


        public override bool IsDone()
        {
            return _movable == null;
        }
    }
}
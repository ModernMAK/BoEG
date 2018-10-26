using System.Collections.Generic;
using Modules.Attackerable;
using Modules.Healthable;
using Modules.Movable;
using Modules.Teamable;
using UnityEngine;

namespace Core.OrderSystem.Order
{
    public class AttackMoveToJob : Job
    {
        public AttackMoveToJob(Vector3 destenation)
        {
            _destenation = destenation;
        }

        private Transform _transform;
        private IMovable _movable;
        private IAttackerable _attackerable;
        private ITeamable _teamable;
        private readonly Vector3 _destenation;

        public override void Initialize(GameObject go)
        {
            _transform = go.transform;
            _movable = go.GetComponent<IMovable>();
            _attackerable = go.GetComponent<IAttackerable>();
            _teamable = go.GetComponent<ITeamable>();
        }

        public override void PreStep(float deltaStep)
        {
            var enemies = ScanForEnemies();
            if (enemies.Length == 0)
            {
                _movable.MoveTo(_destenation);
            }
            else
            {
                _movable.Stop();
                _attackerable.Attack(enemies[0]);
            }
        }

        private GameObject[] ScanForEnemies()
        {
            var enemies = new List<GameObject>();
            var colliders = Physics.OverlapSphere(_transform.position, _attackerable.AttackRange);
            foreach (var collider in colliders)
            {
                var root = collider.attachedRigidbody;
                if(root == null)
                    continue;
                
                var healtable = root.GetComponentInParent<IHealthable>();
                var teamable = root.GetComponentInParent<ITeamable>();

                if (healtable == null)
                    continue;
                if (teamable != null && _teamable != null && _teamable.Team == teamable.Team && _teamable.Team != null)
                    continue;
                if (root.transform == _transform)
                    continue;

                enemies.Add(collider.attachedRigidbody.gameObject);
            }

            enemies.Sort((x, y) =>
                (x.transform.position - _transform.position).sqrMagnitude.CompareTo(
                    (y.transform.position - _transform.position).sqrMagnitude));
            return enemies.ToArray();
        }


        public override bool IsDone()
        {
            return _movable == null;
        }
    }
}
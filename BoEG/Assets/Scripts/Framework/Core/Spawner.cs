using Modules.Teamable;
using UnityEngine;

namespace Framework.Core
{
    public class Spawner : Actor
    {
        [SerializeField] private TeamData _team;
        [SerializeField] private SpawnData[] _data;
        [SerializeField] private Transform _targetPos;
        
        [SerializeField] private Transform[] _spawnLocations;

        private void Start()
        {
            foreach (var data in _data) data.LastSpawn = -data.Interval;
        }

        protected override void Update()
        {
            base.Update();
            foreach (var data in _data)
                if (data.CanSpawn)
                {
                    var spawned = data.Spawn(_spawnLocations);
                    foreach (var go in spawned)
                    {
                        if (go.TryGetComponent<ITeamable>(out var teamable))
                            teamable.SetTeam(_team);

                        if (go.TryGetComponent<UnitController>(out var controller))
                            controller.SetAttackTarget(_targetPos.position);
                    }
                }
        }
    }
}
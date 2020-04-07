using Framework.Core.Modules;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;

namespace Framework.Core
{
    public class Spawner : Actor
    {
        [SerializeField] private SpawnData[] _data;

        private void Start()
        {
            foreach (var data in _data)
            {
                data.LastSpawn = -data.Interval;
            }
        }

        protected override void Update()
        {
            base.Update();
            foreach (var data in _data)
            {
                if (data.CanSpawn)
                {
                    var spawned = data.Spawn();
                }
            }
        }
    }
}
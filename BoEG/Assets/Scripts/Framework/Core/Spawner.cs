using System;
using Framework.Core.Modules;
using UnityEngine;
using UnityEngine.AI;

namespace Framework.Core
{
    public class Spawner : Actor
    {
        [SerializeField] private SpawnData[] _data;

        void Update()
        {
            foreach (var data in _data)
            {
                
            }
        }

    }

    [Serializable]
    public class SpawnData
    {

        public GameObject Prefab;
        public float Interval;
        public float LastSpawn;
        public int SpawnCount;

        public void 
        
        public bool CanSpawn
        {
            get => (Interval + LastSpawn <= Time.time);
        } 
    }
}
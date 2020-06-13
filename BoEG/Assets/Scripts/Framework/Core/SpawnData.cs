using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Framework.Core
{
    [Serializable]
    public class SpawnData
    {
        public float Interval;
        public float LastSpawn;
        public GameObject Prefab;
        public int SpawnCount;

        public bool CanSpawn => Interval + LastSpawn <= Time.time;

        public GameObject[] Spawn(params Vector3[] spawnLocations)
        {
            if (spawnLocations.Length == 0)
                return new GameObject[0];

            var list = new List<GameObject>();
            var offset = Random.Range(0, spawnLocations.Length);
            for (var i = 0; i < SpawnCount; i++)
            {
                var location = spawnLocations[(offset + i) % spawnLocations.Length];
                var go = Spawn(location, Random.insideUnitCircle * Mathf.Epsilon);
                list.Add(go);
            }

            LastSpawn = Time.time;
            return list.ToArray();
        }

        private GameObject Spawn(Vector3 location, Vector2 delta)
        {
            var properDelta = new Vector3(delta.x, 0, delta.y);
            var properLocation = location + properDelta;

            return Object.Instantiate(Prefab, properLocation, Quaternion.identity);
        }
    }
}
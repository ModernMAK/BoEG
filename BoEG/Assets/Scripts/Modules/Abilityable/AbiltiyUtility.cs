using UnityEngine;
using UnityEngine.Networking;

namespace Modules.Abilityable
{
    public static class AbiltiyUtility
    {
        private const float MaxSpawnOffset = 1f;

        public static GameObject[] Spawn(Transform transform, GameObject prefab, int amount = 1)
        {
            return Spawn(transform.position, prefab, amount);
        }


        public static GameObject[] Spawn(Vector3 position, GameObject prefab, int amount = 1)
        {
            if (!NetworkServer.active) 
                return new GameObject[0];
            GameObject[] spawned = new GameObject[amount];
            for (var i = 0; i < amount; i++)
            {
                //TODO replace with proper isServer
                
                spawned[i] = Object.Instantiate(prefab,
                    position + Random.onUnitSphere * ((i + 1) * MaxSpawnOffset / (amount + 1)), Quaternion.identity);
                NetworkServer.Spawn(spawned[i]);
            }

            return spawned;
        }
    }
}
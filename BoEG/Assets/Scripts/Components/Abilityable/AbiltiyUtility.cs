using UnityEngine;
using UnityEngine.Networking;

namespace Components.Abilityable
{
    public static class AbiltiyUtility
    {
        private const float MaxSpawnOffset = 1f;

        public static void Spawn(Transform transform, GameObject prefab, int amount = 1)
        {
            Spawn(transform.position, prefab, amount);
        }

        
        public static void Spawn(Vector3 position, GameObject prefab, int amount = 1)
        {
            for (var i = 0; i < amount; i++)
            {
                //TODO replace with proper isServer
                if (!NetworkServer.active) return;
                var spawned = Object.Instantiate(prefab,
                    position + Random.onUnitSphere * ((i+1) * MaxSpawnOffset / (amount+1)), Quaternion.identity);                
                NetworkServer.Spawn(spawned);
                
            }
        }
    }
}
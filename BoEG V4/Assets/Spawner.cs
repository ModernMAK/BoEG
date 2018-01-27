using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform spawnLocation;

    public void Place(params Movable[] ms)
    {
        foreach (var m in ms)
        {
            m.Teleport(spawnLocation.position);
            m.Turn(spawnLocation.forward, true);
        }
    }

    private void OnDrawGizmos()
    {
        const float SCALE = 0.1f;
        Gizmos.DrawSphere(spawnLocation.position + Vector3.up * SCALE, SCALE);
        Gizmos.DrawRay(spawnLocation.position + Vector3.up * SCALE, spawnLocation.forward);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayDebugger : MonoBehaviour
{
    [SerializeField] private Vector3 _start;
    [SerializeField] private Vector3 _dir;
    [SerializeField] private float _scale;

    private void OnDrawGizmosSelected()
    {
        
        Gizmos.DrawRay(_start,_dir * _scale);
    }
}

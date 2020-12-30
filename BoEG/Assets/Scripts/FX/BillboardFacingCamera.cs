using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardFacingCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private Transform Target => _camera.transform;


    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
    }


    void Update()
    {
        if (_camera != null)
            transform.LookAt(transform.position + Target.forward, Target.up);
    }
}

using UnityEngine;

public class BillboardToCamera : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        var fwd = -_camera.transform.forward;
        transform.LookAt(transform.position + fwd);
    }
}
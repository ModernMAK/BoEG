using UnityEngine;

public class RayDebugger : MonoBehaviour
{
    [SerializeField] private Vector3 _dir;
    [SerializeField] private float _scale;
    [SerializeField] private Vector3 _start;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(_start, _dir * _scale);
    }
}
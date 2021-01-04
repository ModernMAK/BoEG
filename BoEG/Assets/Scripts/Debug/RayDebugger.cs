using UnityEngine;

namespace MobaGame.Debug
{
    public class RayDebugger : MonoBehaviour
    {
#pragma warning disable 0649

        [SerializeField] private Vector3 _dir;
        [SerializeField] private float _scale;
        [SerializeField] private Vector3 _start;
#pragma warning restore 0649

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawRay(_start, _dir * _scale);
        }
    }
}
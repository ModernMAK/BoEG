using UnityEngine;

namespace MobaGame.FX
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        public Transform SetTarget(Transform transform) => _target = transform;

        // Update is called once per frame
        void LateUpdate()
        {
            if (_target != null)
                transform.position = _target.position;
        }
    }
}
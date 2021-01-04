using UnityEngine;

namespace MobaGame.FX
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        private void Awake()
        {
            if (_camera == null)
                _camera = Camera.main;
        }


        void Update()
        {
            if (_camera != null)
                transform.LookAt(_camera.transform);
        }
    }
}
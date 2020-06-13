using UnityEngine;

namespace Util
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private bool _flatten;

        // Update is called once per frame
        private void Update()
        {
            var cam = Camera.main;
            if (_flatten)
                transform.LookAt(transform.position - cam.transform.forward);
            else
                transform.LookAt(cam.transform);
        }
    }
}
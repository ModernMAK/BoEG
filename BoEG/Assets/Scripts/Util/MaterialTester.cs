using UnityEngine;

namespace Util
{
    [ExecuteInEditMode]
    public class MaterialTester : MonoBehaviour
    {
        [SerializeField] private Material _material;

        [SerializeField] private bool _run;

        // Update is called once per frame
        private void Update()
        {
            if (!_run) return;
            _run = false;
            ApplyMaterial();
        }

        private void ApplyMaterial()
        {
            var cmr = transform.GetComponentsInChildren<MeshRenderer>();
            foreach (var mr in cmr)
            {
                mr.material = _material;
            }
            var csmr = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var mr in csmr)
            {
                mr.material = _material;
            }
        }
    }
}
using UnityEngine;

public class PrototypeMaterial : MonoBehaviour
{
#pragma warning disable 0649

    [SerializeField] private Material _protoMaterial;

    [SerializeField] private bool _refresh;
#pragma warning restore 0649
    // Start is called before the first frame update

    // Update is called once per frame
    private void Update()
    {
        if (_refresh)
        {
            _refresh = false;
            Refresh();
        }
    }

    private void Start()
    {
        //True when we have material set, otherwise dont do anything
        _refresh = _protoMaterial != null;
    }

    private void Refresh()
    {
        Refresh(transform);
    }

    private void Refresh(Transform t)
    {
        var mr = t.GetComponent<MeshRenderer>();
        if (mr != null) mr.material = _protoMaterial;

        foreach (Transform child in t) Refresh(child);
    }
}
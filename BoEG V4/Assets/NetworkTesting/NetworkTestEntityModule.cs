using UnityEngine;
using UnityEngine.Networking;

public class NetworkTestEntityModule : NetworkBehaviour
{
    [SyncVar] private Color mColor;

    private MeshRenderer mr;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (mr != null)
            mr.material.color = mColor;
    }

    [Server]
    public void SetColor(Color c)
    {
        mColor = c;
    }
}
using UnityEngine;
using UnityEngine.Networking;

public class NetworkTestEntitySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject PlayerObj;

    private void Awake()
    {
    }

    private void OnPlayerConnected(NetworkPlayer player)
    {
        var go = Instantiate(PlayerObj, Random.onUnitSphere * 10f, Quaternion.identity);
        NetworkServer.Spawn(go);
        var network = go.GetComponent<NetworkTestEntityModule>();
        network.SetColor(Color.HSVToRGB(Random.value, 1f, 1f));
    }

    [Command]
    private void CmdSpawnPlayer()
    {
    }
}
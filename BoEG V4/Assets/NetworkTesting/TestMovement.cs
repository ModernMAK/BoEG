using UnityEngine;
using UnityEngine.Networking;

public class TestMovement : NetworkBehaviour
{
    public float Speed = 8;

    // Use this for initialization
    private void Start()
    {
        Speed = Mathf.Max(0f, Speed);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isLocalPlayer)
            return;
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(x, 0, y).normalized * Speed * Time.deltaTime, Space.World);
    }
}
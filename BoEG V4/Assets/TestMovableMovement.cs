using UnityEngine;
using UnityEngine.Networking;

public class TestMovableMovement : NetworkBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
                if (GetComponent<Movable>().MoveTo(hit.point))
                    Debug.Log("Moving to " + hit.point);
        }
        if (Input.GetKeyDown(KeyCode.C))
            GetComponent<Movable>().Cancel();
    }
}
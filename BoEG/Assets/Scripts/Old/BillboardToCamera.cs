using UnityEngine;

public class BillboardToCamera : MonoBehaviour {

	private void Update()
	{
		var fwd = -Camera.main.transform.forward;
		transform.LookAt(transform.position + fwd);
	}
}

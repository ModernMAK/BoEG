using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAoeVisualizer : MonoBehaviour {

	public void SetAoeSize(float radius)
	{
		transform.localScale = Vector3.one * radius;
	}

	public void SetPoint(Vector3 point)
	{
		transform.position = point + Vector3.up / 100f;
	}
}

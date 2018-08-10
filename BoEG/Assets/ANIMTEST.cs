using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANIMTEST : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position += transform.forward * Time.deltaTime;
	}
}

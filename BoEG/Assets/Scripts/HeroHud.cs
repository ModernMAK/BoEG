using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHud : MonoBehaviour
{

	//USED FOR DEBUG ONLY
	[SerializeField]
	private GameObject _target;

	private AbilityPanel _abilityPanel;
	
	void Awake()
	{
		_abilityPanel = GetComponentInChildren<AbilityPanel>();
		if (_target != null)
			SetTarget(_target);
	}

	public void SetTarget(GameObject go)
	{
		_abilityPanel.Set(go);
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

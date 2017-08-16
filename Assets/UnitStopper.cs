using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStopper : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Monster" && other.GetComponent<Monster> ().Attacker) 
		{
			other.GetComponent<Monster> ().Speed = 0;
		}
	}
}

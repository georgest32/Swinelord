using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablerUnit : Monster {

	// Use this for initialization
	void Start () {
		
	}

	public void DisableTower(Tower target){
		Debug.Log ("Disabling: " + Target.GetComponentInChildren<Tower> ().gameObject);

		target.DisabledByUnit = true;
	}

	public override UnitDebuff GetUnitDebuff()
	{
		return new DisablerDebuff(Target, DebuffDuration);
	}

	protected override void Attack()
	{
		DisableTower (Target);

		base.Attack ();
	}
}

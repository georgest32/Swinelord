using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablerDebuff : UnitDebuff {

	private float slowingFactor;

	private bool applied;

	public DisablerDebuff(Tower target, float duration) : base(target, duration)
	{
		Debug.Log ("Disabling: " + target.GetComponentInChildren<Tower> ().gameObject);

		target.DisabledByUnit = true;
	}

	public override void Update(){


		base.Update ();
	}

	public override void Remove(){
		
		target.DisabledByUnit = false;

		base.Remove ();
	}
}

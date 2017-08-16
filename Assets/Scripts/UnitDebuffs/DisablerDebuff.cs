using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablerDebuff : UnitDebuff {

	private float slowingFactor;

	private bool applied;

	public DisablerDebuff(Tower target, float duration) : base(target, duration)
	{
		this.slowingFactor = slowingFactor;
	}

	public override void Update(){


		base.Update ();
	}

	public override void Remove(){

		base.Remove ();
	}
}

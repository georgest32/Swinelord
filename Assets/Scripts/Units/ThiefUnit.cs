using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefUnit : Monster {

	// Use this for initialization
	void Start () {
		
	}
	


	public override UnitDebuff GetUnitDebuff()
	{
		return new DisablerDebuff(Target, DebuffDuration);

	}
}

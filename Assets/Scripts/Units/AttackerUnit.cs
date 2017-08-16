using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerUnit : Monster {

	// Use this for initialization
	void Start () {
		
	}

	public override UnitDebuff GetUnitDebuff()
	{
		return new AttackerDebuff(Target, DebuffDuration);

	}
}

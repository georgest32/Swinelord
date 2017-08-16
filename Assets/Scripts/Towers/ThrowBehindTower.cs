using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBehindTower : Tower {

	private void Start(){
		this.ElementType = Element.PHYSICAL;
	}

	public override Debuff GetDebuff()
	{
		return new PhysicalDebuff(Target, DebuffDuration);

	}
}

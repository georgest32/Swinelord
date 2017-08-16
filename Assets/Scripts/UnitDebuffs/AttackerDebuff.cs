using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerDebuff : UnitDebuff {

	public AttackerDebuff(Tower target, float duration) : base(target, duration)
	{
	}

	public override void Update(){


		base.Update ();
	}

	public override void Remove(){


		base.Remove ();
	}
}

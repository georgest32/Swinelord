using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitDebuff 
{
	private float duration;

	private float elapsed;

	protected Tower target;

	public UnitDebuff (Tower target, float duration)
	{
		this.target = target;
		this.duration = duration;
	}

	public virtual void Update()
	{
		elapsed += Time.deltaTime;

		if (elapsed >= duration) {
			Remove ();
		}
	}

	public virtual void Remove(){

		if (target != null) {
			Debug.Log ("removing " + this);
			target.UnitDebuffsToRemove.Add(this);
		}

	}
}

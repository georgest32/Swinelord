using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransfattyTower : Tower {

	[SerializeField]
	private float slowingFactor;

	public float SlowingFactor {
		get {
			return slowingFactor;
		}
	}

	private void Start(){
		this.ElementType = Element.ACID;

		TowerUpgrades = new TowerUpgrade[] {
			new TowerUpgrade (2, 1, 2, 25, 10),
			new TowerUpgrade (2, 1, 2, 25, 10),
		};
	}

	public override Debuff GetDebuff()
	{
		return new AcidDebuff(slowingFactor, DebuffDuration, Target);

	}

	public override string GetStats ()
	{
		if (NextUpgrade != null) 
		{
			return string.Format ("<color=#ffa500ff>{0}</color>{1}", "<size=20><b>TransfattyTower Acids</b></size> ", base.GetStats());
		}

		return string.Format ("<color=#00ff00ff>{0}</color>{1}", "<Size=20><b>Transfatty Acids</b></size>", base.GetStats());
	}

	public override void Upgrade()
	{
		this.slowingFactor += NextUpgrade.SlowingFactor;

		base.Upgrade ();
	}
}

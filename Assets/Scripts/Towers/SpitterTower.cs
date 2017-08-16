using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterTower : Tower {

	private void Start(){
		this.ElementType = Element.PHYSICAL;

		TowerUpgrades = new TowerUpgrade[] {
			new TowerUpgrade (2, 1, 0.5f, 5),
			new TowerUpgrade (2, 1, 0.5f, 5),
		};
	}

	public override Debuff GetDebuff()
	{
		return new PhysicalDebuff(Target, DebuffDuration);

	}

	public override string GetStats ()
	{
		if (NextUpgrade != null) 
		{
			return string.Format ("<color=#ffa500ff>{0}</color>{1}", "<size=20><b>Spitter</b></size> ", base.GetStats());
		}

		return string.Format ("<color=#00ff00ff>{0}</color>{1}", "<Size=20><b>Spitter</b></size>", base.GetStats());
	}

	public override void Upgrade()
	{
		
		base.Upgrade ();
	}
}

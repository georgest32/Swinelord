using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwinelordTower : Tower {

	private void Start(){
		this.ElementType = Element.HALLOWED;

		//Upgrade stats are wrong, need to define Hallowed
		//Proc chance to instantly kill unit?
		//Raccoon that reflects attacks to kill?
		TowerUpgrades = new TowerUpgrade[] {
			new TowerUpgrade (2, 2, 2, 2, 15, 1),
			new TowerUpgrade (2, 2, 2, 2, 15, 1),
			new TowerUpgrade (2, 2, 2, 2, 15, 1),	//Lord of the Sties

		};
	}

	public override Debuff GetDebuff()
	{
		return new HallowedDebuff(Target);

	}
		
	public override string GetStats ()
	{
		if (NextUpgrade != null) 
		{
			return string.Format ("<color=#ffa500ff>{0}</color>{1}", "<size=20><b>Swinelord</b></size> ", base.GetStats());
		}

		return string.Format ("<color=#00ff00ff>{0}</color>{1}", "<Size=20><b>Swinelord</b></size>", base.GetStats());
	}

	public override void Upgrade()
	{

		base.Upgrade ();
	}
}

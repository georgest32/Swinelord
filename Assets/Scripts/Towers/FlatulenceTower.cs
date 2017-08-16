using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatulenceTower : Tower {

	[SerializeField]
	private float tickTime;

	[SerializeField]
	private float tickDamage;

	public float TickTime
	{
		get {
			return tickTime;
		}
	}

	public float TickDamage
	{
		get {
			return tickDamage;
		}
	}

	private void Start(){
		this.ElementType = Element.FIRE;

		TowerUpgrades = new TowerUpgrade[] {
			new TowerUpgrade (2, 2, 2, 2, 20, 0.5f),
			new TowerUpgrade (2, 2, 2, 2, 20, 0.5f),
			new TowerUpgrade (2, 2, 2, 2, 20, 0.5f),

		};
	}

	public override Debuff GetDebuff()
	{
		return new FireDebuff (TickDamage, tickTime, DebuffDuration, Target);
	}

	public override string GetStats ()
	{
		if (NextUpgrade != null) 
		{
			return string.Format ("<color=#ffa500ff>{0}</color>{1}", "<size=20><b>Flatulation Conflagration</b></size> ", base.GetStats());
		}

		return string.Format ("<color=#00ff00ff>{0}</color>{1}", "<Size=20><b>Flatulation Conflagration</b></size>", base.GetStats());
	}

	public override void Upgrade()
	{
		this.tickTime -= NextUpgrade.TickTime;
		this.tickDamage += NextUpgrade.SpecialDamage;

		base.Upgrade ();
	}
}

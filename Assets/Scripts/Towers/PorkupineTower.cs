using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorkupineTower : Tower {

	[SerializeField]
	private float tickTime;

	[SerializeField]
	private PoisonSplash splashPrefab;

	[SerializeField]
	private int splashDamage;

	public float TickTime {
		get {
			return tickTime;
		}
	}

	public int SplashDamage {
		get {
			return splashDamage;
		}
	}

	private void Start(){
		this.ElementType = Element.POISON;

		TowerUpgrades = new TowerUpgrade[] {
			new TowerUpgrade (2, 2, 2, 2, 15, 1),
			new TowerUpgrade (2, 2, 2, 2, 15, 1),
			new TowerUpgrade (2, 2, 2, 2, 15, 1),

		};
	}

	public override Debuff GetDebuff()
	{
		return new PoisonDebuff(splashDamage, tickTime, splashPrefab, DebuffDuration, Target);

	}

	public override string GetStats ()
	{
		if (NextUpgrade != null) 
		{
			return string.Format ("<color=#ffa500ff>{0}</color>{1}", "<size=20><b>Porkupine</b></size> ", base.GetStats());
		}

		return string.Format ("<color=#00ff00ff>{0}</color>{1}", "<Size=20><b>Porkupine</b></size>", base.GetStats());
	}

	public override void Upgrade()
	{
		this.tickTime -= NextUpgrade.TickTime;
		this.splashDamage += NextUpgrade.SpecialDamage;

		base.Upgrade ();
	}
}

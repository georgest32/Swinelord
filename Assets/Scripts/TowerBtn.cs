using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour {

	[SerializeField]
	private GameObject towerPrefab;

	[SerializeField]
	private Sprite sprite;

	[SerializeField]
	private int price;

	[SerializeField]
	private Text priceText;

	public GameObject TowerPrefab { get{ return towerPrefab; } }

	public Sprite Sprite { get { return sprite; } }

	public int Price { get { return price; } }

	private void Start()
	{
		priceText.text = price.ToString () + " <color=yellow>$</color>";

		GameManager.Instance.Changed += new CurrencyChanged (PriceCheck);
	}
		
	private void PriceCheck(){
		if (price <= GameManager.Instance.Currency) {
			GetComponent<Image> ().color = Color.white;
			priceText.color = Color.white;
		} 
		else 
		{
			GetComponent<Image> ().color = Color.grey;
			priceText.color = Color.grey;
		}
	}

	public void ShowInfo(string type)
	{
		string toolTip = string.Empty;

		switch (type) 
		{
			case "Wall":
			toolTip = string.Format ("<color=#fff><size=20><b>Wall</b></size></color>\nA barrier with which to maze.");

				break;

			case "Swine":
				SwineTower swine = towerPrefab.GetComponentInChildren<SwineTower> ();

			toolTip = string.Format ("<color=#ffa500ff><size=20><b>Swine</b></size></color>\nDamage: {0} \nProc: {1}% \nDebuff duration: {2}sec \nAttacks have a chance to concuss the target, stunning it for 3 seconds.", 
										swine.Damage, swine.Proc, swine.DebuffDuration);
			
				break;

			case "Spitter":
				SpitterTower spitter = towerPrefab.GetComponentInChildren<SpitterTower> ();

			toolTip = string.Format ("<color=#00ffffff><size=20><b>Spitter</b></size></color>\nDamage: {0} \nProc: {1}% \nDebuff duration: {2}sec \n\nAttacks apply a stacking debuff that increase the target's damage taken.", 
										spitter.Damage, spitter.Proc, spitter.DebuffDuration);
			
				break;

			case "TransfattyAcids":
				TransfattyTower transfattyTower = towerPrefab.GetComponentInChildren<TransfattyTower> ();

			toolTip = string.Format ("<color=#00ffffff><size=20><b>Transfatty Acids</b></size></color>\nDamage: {0} \nProc: {1}% \nDebuff duration: {2}sec \nSlowing factor: {3}% \nHas a chance to slow down the target.", 
										transfattyTower.Damage, transfattyTower.Proc, transfattyTower.DebuffDuration, transfattyTower.SlowingFactor);
			
				break;

			case "FlatulationConflagration":
				FlatulenceTower fart = towerPrefab.GetComponentInChildren<FlatulenceTower> ();

			toolTip = string.Format ("<color=#00ff00ff><size=20><b>Flatulation Conflagration</b></size></color>\nDamage: {0} \nProc: {1}% \nDebuff duration: {2}sec \nIgnites the target, dealing extra damage.", 
				fart.Damage, fart.Proc, fart.DebuffDuration);
			
				break;

			case "Porkupine":
				PorkupineTower pork = towerPrefab.GetComponentInChildren<PorkupineTower> ();

			toolTip = string.Format ("<color=#add8e6ff><size=20><b>Porkupine</b></size></color>\nDamage: {0} \nProc: {1}% \nDebuff duration: {2}sec \nApplies a corrosive toxin to the target, which is spread around the map by the target's movement.", 
										pork.Damage, pork.Proc, pork.DebuffDuration);
			
				break;

			case "Swinelord":
				SwinelordTower swinelord = towerPrefab.GetComponentInChildren<SwinelordTower> ();

			toolTip = string.Format ("<color=#ff0f0ff><size=20><b>Swinelord</b></size></color>\nDamage: {0} \nProc: {1}% \nDebuff duration: {2}sec \nIdk yet.", 
										swinelord.Damage, swinelord.Proc, swinelord.DebuffDuration);
			
				break;
		}
		
		GameManager.Instance.SetTooltipText (toolTip);
		GameManager.Instance.ShowStats ();
	}
}

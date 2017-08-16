using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public delegate void CurrencyChanged();

public class GameManager : Singleton<GameManager> {

	public event CurrencyChanged Changed;

	public TowerBtn ClickedBtn { get; set; }

	private int currency;

	private int wave = 0;

	private int lives;

	private bool gameOver = false;

	public GameObject SelectedPortal { get; set; }

	public bool ChangingPortal { get; set; }

	[SerializeField]
	private GameObject statsPanel;

	[SerializeField]
	private GameObject gameOverMenu;

	[SerializeField]
	private Text livesText;

	[SerializeField]
	private Text waveText;

	[SerializeField]
	private Text currencyText;

	[SerializeField]
	private GameObject waveButton;

	[SerializeField]
	private GameObject upgradePanel;

	[SerializeField]
	private Text sellText;

	[SerializeField]
	private Text statText;

	[SerializeField]
	private Text upgradePrice;

	private Tower selectedTower;

	private List<Monster> activeMonsters = new List<Monster> ();

	public ObjectPool Pool { get; set; }

	public bool WaveActive
	{
		get
		{
			return activeMonsters.Count > 0;
		}
	}

	public int Currency
	{
		get 
		{
			return currency;
		}
		set 
		{
			this.currency = value;
			this.currencyText.text = value.ToString () + " <color=yellow>$</color>";

			OnCurrencyChanged ();
		}
	}

	public int Lives
	{
		get 
		{
			return lives;
		}
		set 
		{
			this.lives = value;
		
			if (lives <= 0) 
			{
				this.lives = 0;
				GameOver ();
			}

			livesText.text = lives.ToString ();
		}
	}

	private void Awake ()
	{
		Pool = GetComponent<ObjectPool> ();
	}

	void Start () 
	{
		Lives = 10;
		Currency = 25;
		ChangingPortal = false;
	}
	
	void Update () 
	{
		HandleEscape ();
	}

	public void PickTower(TowerBtn towerBtn)
	{
		
		if (Currency >= towerBtn.Price && !WaveActive) 
		{
			this.ClickedBtn = towerBtn;
			Hover.Instance.Activate (towerBtn.Sprite);
		}
	}

	public void BuyTower()
	{

		if (Currency >= ClickedBtn.Price) 
		{
			Currency -= ClickedBtn.Price;
		}

		Hover.Instance.Deactivate ();
	}

	public void OnCurrencyChanged()
	{
		if(Changed != null)
		{
			Changed ();
		}
	}

	private void HandleEscape()
	{

		if (Input.GetKeyDown (KeyCode.Escape)) 
		{

			Hover.Instance.Deactivate ();
		}
	}

	public void StartWave(string type)
	{
		LevelManager.Instance.GeneratePathBlueToRed ();

		Monster monster = Pool.GetObject (type).GetComponent<Monster> ();

		monster.Spawn ();

		activeMonsters.Add (monster);
	}

//	private IEnumerator SpawnWave()
//	{
//
//
////			yield return new WaitForSeconds (2.5f);
//	}

	public void RemoveMonster(Monster monster)
	{
		activeMonsters.Remove (monster);
	
	}

	public void GameOver()
	{
		if (!gameOver) 
		{
			gameOverMenu.SetActive (true);
		}
	}

	public void Restart()
	{
		Time.timeScale = 1;

		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void QuitGame()
	{
		Application.Quit ();
	}

	public void SelectTower(Tower tower)
	{
		//if a tower is already selected, deselect it
		if (selectedTower != null) {
			selectedTower.Select ();
		}
		
		selectedTower = tower;
		selectedTower.Select ();

		sellText.text = "+" + (selectedTower.Price / 2).ToString () + " $";

		upgradePanel.SetActive (true);
	
	}

	public void DeselectTower()
	{
		
		if (selectedTower != null) 
		{
			selectedTower.Select ();
		}

		upgradePanel.SetActive (false);

		selectedTower = null;
	}

	public void SellTower()
	{
		if (selectedTower != null) 
		{
			Currency += selectedTower.Price / 2;

			selectedTower.GetComponentInParent<TileScript> ().IsEmpty = true;

			Destroy (selectedTower.transform.parent.gameObject);

			DeselectTower ();
		}
	}

	public void ShowStats()
	{
		statsPanel.SetActive (!statsPanel.activeSelf);
	}

	public void SetTooltipText(string txt)
	{
		statText.text = txt;
	}

	public void UpdateUpgradeTooltip()
	{
		if (selectedTower != null) 
		{
			sellText.text = "+ " + (selectedTower.Price/2).ToString() + " $";
			SetTooltipText (selectedTower.GetStats());

			if (selectedTower.NextUpgrade != null) 
			{
				upgradePrice.text = "- " + selectedTower.NextUpgrade.Price.ToString () + " $";
			} 
			else 
			{
				upgradePrice.text = string.Empty;
			}
		}

	}

	public void ShowSelectedTowerStats()
	{
		statsPanel.SetActive (!statsPanel.activeSelf);
		UpdateUpgradeTooltip ();
	}

	public void UpgradeTower()
	{
		if (selectedTower != null) 
		{
			if (selectedTower.Level <= selectedTower.TowerUpgrades.Length && Currency >= selectedTower.NextUpgrade.Price) 
			{
				selectedTower.Upgrade ();
			}
		}
	}

	public void EnterBaconraid()
	{
		SceneManager.LoadScene ("Baconraid");
	}

	public void EndBaconraid()
	{
		SceneManager.LoadScene ("Home");
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Element {PHYSICAL, ACID, POISON, FIRE, HALLOWED, NONE}

public abstract class Tower : MonoBehaviour {

	[SerializeField]
	private string projectileType;

	[SerializeField]
	private float projectileSpeed;

	[SerializeField]
	private int damage;

	[SerializeField]
	private float debuffDuration;

	[SerializeField]
	private float proc;

	[SerializeField]
	private float attackCooldown;

	[SerializeField]
	private Stat health;

	[SerializeField]
	private int hitPoints;

	private int level;

	private Animator myAnimator;

	private List<UnitDebuff> unitDebuffs = new List<UnitDebuff> ();

	public List<UnitDebuff> UnitDebuffsToRemove { get; private set; }

	public List<UnitDebuff> NewUnitDebuffs = new List<UnitDebuff>();

	private SpriteRenderer mySpriteRenderer;

	private Monster target;

	private Queue<Monster> monsters = new Queue<Monster> ();

	private bool canAttack = true;

	private bool disabledByUnit = false;

	private float attackTimer;

	public int Level { get; set; }

	public bool IsActive { get; set; }

	public TowerUpgrade[] TowerUpgrades { get; protected set; }

	public Element ElementType { get; protected set; }

	public int Price { get; set; }

	public float ProjectileSpeed { get { return projectileSpeed; } }

	public Monster Target { get{ return target; } }

	public int HitPoints {
		get {
			return hitPoints;
		}
		set {
			hitPoints = value;
		}
	}

	public bool IsAlive 
	{
		get 
		{
			return health.CurrentValue > 0;
		}
	}


	public bool DisabledByUnit 
	{
		get 
		{
			return disabledByUnit;
		}
		set 
		{
			disabledByUnit = value;
		}
	}

	public bool CanAttack 
	{
		get 
		{
			return canAttack;
		}
		set 
		{
			canAttack = value;
		}
	}

	public TowerUpgrade NextUpgrade
	{
		get 
		{
			if (TowerUpgrades.Length > Level - 1) 
			{
				return TowerUpgrades [Level - 1];
			}

			return null;
		}
	}

	public int Damage
	{
		get 
		{
			return damage;
		}
	}

	public float DebuffDuration 
	{ 
		get 
		{ 
			return debuffDuration; 
		}
		set 
		{
			this.debuffDuration = value;
		}
	}

	public float Proc 
	{ 
		get 
		{ 
			return proc; 
		}
		set 
		{
			this.proc = value;
		}
	}

	// Use this for initialization
	void Awake () 
	{
		myAnimator = transform.parent.GetComponent<Animator> ();
		mySpriteRenderer = GetComponent<SpriteRenderer> ();
		Level = 1;

		UnitDebuffsToRemove = new List<UnitDebuff>();

		this.health.MaxVal = this.hitPoints;
		this.health.CurrentValue = this.health.MaxVal;

		this.health.Initialize ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Attack ();
		HandleDebuffs ();

		if (health.CurrentValue == health.MaxVal && transform.GetChild (0).gameObject.activeInHierarchy) 
		{
			transform.GetChild (0).gameObject.SetActive (false);
		}
		else if (health.CurrentValue < health.MaxVal && !transform.GetChild (0).gameObject.activeInHierarchy) 
		{
			transform.GetChild (0).gameObject.SetActive (true);
		}
	}

	public void Select()
	{
		mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
		GameManager.Instance.UpdateUpgradeTooltip ();
	}

	private void Attack()
	{
		if (!disabledByUnit && this.transform.parent.tag != "Wall") 
		{
			if (!canAttack) 
			{
				attackTimer += Time.deltaTime;

				if (attackTimer >= attackCooldown) 
				{
					canAttack = true;
					attackTimer = 0;
				}
			}

			if (target == null && monsters.Count > 0) 
			{
				target = monsters.Dequeue ();
			}

			if (target != null && target.IsActive) 
			{
				if (canAttack) {
					Shoot ();
					myAnimator.SetTrigger ("Attack");
					canAttack = false;
				}
			} 

			if (target != null && !target.IsAlive || target != null && !target.IsActive) 
			{
				target = null;
			}
		}
	}

	public virtual string GetStats()
	{
		if (NextUpgrade != null) 
		{
			return string.Format ("\nLevel: {0} \nDamage: {1} <color=#00ff00ff> + {4}</color>\nProc: {2}% <color=#00ff00ff> + {5}%</color>\nDebuff: {3} sec <color=#00ff00ff> + {6} sec</color>", Level, damage, proc, DebuffDuration, NextUpgrade.Damage, NextUpgrade.ProcChance, NextUpgrade.DebuffDuration);
		}

		return string.Format ("\nLevel: {0} \nDamage: {1} \nProc: {2}% \nDebuff: {3}sec", Level, damage, proc, DebuffDuration);
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Monster") 
		{
			monsters.Enqueue (other.GetComponent<Monster> ());
		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Monster") 
		{
			target = null;
		}
	}

	private void Shoot()
	{
		Projectile projectile = GameManager.Instance.Pool.GetObject (projectileType).GetComponent<Projectile>();

		projectile.transform.position = transform.position;

		projectile.Initialize (this);
	}

	public virtual void Upgrade()
	{
		GameManager.Instance.Currency -= NextUpgrade.Price;
		Price += NextUpgrade.Price;
		this.damage += NextUpgrade.Damage;
		this.proc += NextUpgrade.ProcChance;
		this.debuffDuration += NextUpgrade.DebuffDuration;
		Level++;
		GameManager.Instance.UpdateUpgradeTooltip ();
	}

	public void TakeDamage(float damage)
	{
		if (IsAlive) 
		{
			this.health.CurrentValue -= damage;

			if (health.CurrentValue <= 0) {

//				myAnimator.SetTrigger ("Die");

				this.transform.parent.parent.GetComponent<TileScript> ().IsEmpty = true;

				this.transform.parent.parent.GetComponent<TileScript> ().Walkable = true;

				GameObject.Destroy (transform.parent.gameObject);
			}
		}
	}

	public void AddDebuff(UnitDebuff debuff)
	{
		if(!unitDebuffs.Exists(x => x.GetType() == debuff.GetType()))
		{
			NewUnitDebuffs.Add (debuff);
		}

	}

	private void HandleDebuffs()
	{
		//If the monster has any new debuffs
		if (NewUnitDebuffs.Count > 0)
		{
			//Then we add them to the debuffs list
			unitDebuffs.AddRange(NewUnitDebuffs);

			//Then clear new debuffs so that they only will be added once
			NewUnitDebuffs.Clear();
		}

		//Checks if we need to remove any debuffs
		foreach (UnitDebuff debuff in UnitDebuffsToRemove)
		{
			//If so then remove it
			unitDebuffs.Remove(debuff);
		}

		//Clears the debuffs to remove, so that we only try to remove them once
		UnitDebuffsToRemove.Clear();

		//Updates all debuffs
		foreach (UnitDebuff debuff in unitDebuffs) 
		{
			debuff.Update();
		}
	}

	public void RemoveDebuff(UnitDebuff debuff)
	{
		UnitDebuffsToRemove.Add (debuff);
		unitDebuffs.Remove (debuff);
	}

	public abstract Debuff GetDebuff ();
}

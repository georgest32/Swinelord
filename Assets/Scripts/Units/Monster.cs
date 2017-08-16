using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour 
{
	[SerializeField]
	private float debuffDuration;

	[SerializeField]
	private float speed;

	[SerializeField]
	private bool attacker;

	[SerializeField]
	private int bacon;

	[SerializeField]
	private int damage;

	[SerializeField]
	private int hitPoints;

	[SerializeField]
	private float attackCooldown;

	[SerializeField]
	private Element elementType;

	[SerializeField]
	private Stat health;

	private bool canAttack = true;

	private Stack<Node> path;

	private List<Debuff> debuffs = new List<Debuff> ();

	public List<Debuff> DebuffsToRemove { get; private set; }

	private List<Debuff> NewDebuffs = new List<Debuff>();

	private Point targetLocation;

	private bool stoleBacon = false;

	private bool atTarget = false;

	public Point GridPosition { get; set; }

	public float MaxSpeed { get; set; }

	private Vector3 destination;

	public bool IsActive { get; set; }

	private Animator myAnimator;

	private SpriteRenderer spriteRenderer;

	private int invulnerability = 2;

	private Tower target;

	private Queue<Tower> towers = new Queue<Tower> ();

	private float attackTimer;

	public Point TargetLocation 
	{
		get 
		{
			return targetLocation;
		}
		set 
		{
			this.targetLocation = value;
		}
	}

	public bool StoleBacon 
	{
		get 
		{
			return stoleBacon;
		}
		set 
		{
			this.stoleBacon = value;
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

	public Tower Target {
		get 
		{
			return target;
		}
		set 
		{
			this.target = value;
		}
	}

	public bool Attacker {
		get 
		{
			return attacker;
		}
		set 
		{
			this.attacker = value;
		}
	}

	public int Damage
	{
		get 
		{
			return damage;
		}
	}

	public int Bacon 
	{
		get 
		{
			return bacon;
		}
		set 
		{
			this.bacon = value;
		}
	}

	public float Speed 
	{
		get 
		{
			return speed;
		}
		set 
		{
			this.speed = value;
		}
	}

	public bool IsAlive 
	{
		get 
		{
			return health.CurrentValue > 0;
		}
	}

	public Element ElementType 
	{
		get 
		{
			return elementType;
		}
		set 
		{
			this.elementType = value;
		}
	}

	void Awake()
	{
		DebuffsToRemove = new List<Debuff>();

		MaxSpeed = speed;

		myAnimator = GetComponent<Animator> ();

		spriteRenderer = GetComponent<SpriteRenderer> ();

		health.Initialize ();
	}

	private void Update()
	{
		HandleDebuffs ();
		Move ();
		Debug.Log ("atTarget: " + atTarget);

		if (Attacker && atTarget || Attacker && towers.Count > 0) 
		{
			Debug.Log ("Target: " + target);
			Attack ();
		}

		if (atTarget && target == null && towers.Count == 0) 
		{
			speed = MaxSpeed;

			if (stoleBacon) 
			{
				Stack<Node> newPath = LevelManager.Instance.GeneratePathToNextPortal (this, LevelManager.Instance.BlueSpawn);
				SetPath (newPath);	
			} 
			else 
			{
				Stack<Node> newPath = LevelManager.Instance.GeneratePathToNextPortal (this, LevelManager.Instance.RedSpawn);
				SetPath (newPath);
			}

			atTarget = false;
		}
	}

	public void Spawn()
	{
		target = null; 
		atTarget = false;
		speed = MaxSpeed;

		this.stoleBacon = false;
		transform.position = LevelManager.Instance.BluePortal.transform.position;

		this.health.Bar.Reset ();
		this.health.MaxVal = hitPoints;
		this.health.CurrentValue = this.health.MaxVal;

		StartCoroutine (Scale (new Vector3 (0.1f, 0.1f), new Vector3 (1, 1), false));

		SetPath (LevelManager.Instance.Path);
	}

	public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
	{
		float progress = 0;

		while (progress <= 1) 
		{
			transform.localScale = Vector3.Lerp (from, to, progress);

			progress += Time.deltaTime;

			yield return null;
		}

		transform.localScale = to;

		IsActive = true;

		if (remove == true) {
			Release ();
		}
	}

	private void Move()
	{
		if (IsActive) 
		{
			transform.position = Vector2.MoveTowards (transform.position, destination, speed * Time.deltaTime);

			if (transform.position == destination) 
			{
				
				if (path != null && path.Count > 0) 
				{
					Animate (GridPosition, path.Peek ().GridPosition);

					GridPosition = path.Peek ().GridPosition;

					destination = path.Pop ().WorldPosition;

				}
			}
		}
	}

	private void SetPath(Stack<Node> newPath)
	{
		Debug.Log ("Setting path");
		if (newPath != null) 
		{
			this.path = newPath;

			Animate (GridPosition, path.Peek ().GridPosition);

			GridPosition = path.Peek ().GridPosition;
		
			destination = path.Pop ().WorldPosition;
		}
	}

	private void Animate(Point currentPosition, Point nextPosition)
	{
		if (currentPosition.Y > nextPosition.Y) 
		{
			//down

			myAnimator.SetFloat ("Horizontal", 0);
			myAnimator.SetFloat ("Vertical", 1);

		}
		else if (currentPosition.Y < nextPosition.Y) 
		{
			//up

			myAnimator.SetFloat ("Horizontal", 0);
			myAnimator.SetFloat ("Vertical", -1);

		}

		if (currentPosition.Y == nextPosition.Y) 
		{
			if (currentPosition.X < nextPosition.X) 
			{

				myAnimator.SetFloat ("Horizontal", 1);
				myAnimator.SetFloat ("Vertical", 0);

			}
			else if (currentPosition.X > nextPosition.X) 
			{

				myAnimator.SetFloat ("Horizontal", -1);
				myAnimator.SetFloat ("Vertical", 0);

			}
		}
	}

	private void Attack()
	{
		Debug.Log (towers.Count);

		if (!canAttack) 
		{
			attackTimer += Time.deltaTime;

			if (attackTimer >= attackCooldown) 
			{
				canAttack = true;
				attackTimer = 0;
			}
		}

		if (Target != null && Target.IsActive && atTarget)
		{
			if (canAttack)
			{
				target.TakeDamage (this.damage);
		
				target.AddDebuff (this.GetUnitDebuff ());

				myAnimator.SetTrigger("Attack");

				canAttack = false;
			}
		}

		if (!target.IsAlive) {
			if (towers.Count > 0 && target == null)
			{
				towers.Dequeue ();

				speed = MaxSpeed;

				target = towers.Peek();
			}

			else if (towers.Count == 0) 
			{
				if (stoleBacon) 
				{
					Stack<Node> newPath = LevelManager.Instance.GeneratePathToNextPortal (this, LevelManager.Instance.BlueSpawn);
					SetPath (newPath);	
				} 
				else 
				{
					Stack<Node> newPath = LevelManager.Instance.GeneratePathToNextPortal (this, LevelManager.Instance.RedSpawn);
					SetPath (newPath);
				}
			}
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Tower" && Attacker) 
		{
			towers.Enqueue (other.GetComponent<Tower> ());

			if (target == null) 
			{
				target = other.GetComponentInChildren<Tower> ();

				Stack<Node> newPath = LevelManager.Instance.GeneratePathToTarget (this, target.transform.parent.parent.GetComponent<TileScript>());

				SetPath(newPath);
			}
		}

		else if (other.tag == "UnitStopper" && Attacker && target != null) 
		{
			Debug.Log ("Stopping");
			speed = 0;
			atTarget = true;
		}

		else if (other.tag == "BluePortal" && stoleBacon ) 
		{
			StartCoroutine(Scale(new Vector3(1,1), new Vector3(0.1f,0.1f), true));

//			other.GetComponent<Portal> ().Open ();
		}

		else if (other.tag == "RedPortal" && !stoleBacon ) 
		{
			this.bacon += 10;
			this.stoleBacon = true;

			Stack<Node> newPath = LevelManager.Instance.GeneratePathRedToBlue ();

			SetPath(newPath);
		}

		else if (other.tag == "Tile") 
		{
			spriteRenderer.sortingOrder = other.GetComponent<TileScript> ().GridPosition.Y;
		}
	}

	public void Release()
	{
		debuffs.Clear ();

		IsActive = false;

		GridPosition = LevelManager.Instance.BlueSpawn;

		GameManager.Instance.Pool.ReleaseObject (gameObject);

		GameManager.Instance.RemoveMonster (this);
	
	}

	public void TakeDamage(float damage, Element damageSource)
	{
		if (IsActive) 
		{
			health.CurrentValue -= damage;

			if (health.CurrentValue <= 0) {

				GameManager.Instance.Currency += 2;

				myAnimator.SetTrigger ("Die");

				IsActive = false;

				GetComponent<SpriteRenderer> ().sortingOrder--;

			}
		}
	}

	public void AddDebuff(Debuff debuff)
	{
		if(!debuffs.Exists(x => x.GetType() == debuff.GetType()))
		{
			NewDebuffs.Add (debuff);
		}
	}

	private void HandleDebuffs()
	{
		if (NewDebuffs.Count > 0) 
		{
			debuffs.AddRange (NewDebuffs);

			NewDebuffs.Clear();
		}

		foreach (Debuff debuff in DebuffsToRemove) 
		{
			debuffs.Remove (debuff);
		}

		DebuffsToRemove.Clear ();

		foreach (Debuff debuff in debuffs) 
		{
			debuff.Update ();
		}
	}

	public void RemoveDebuff(Debuff debuff)
	{
		DebuffsToRemove.Add (debuff);
		debuffs.Remove (debuff);
	}

	public abstract UnitDebuff GetUnitDebuff ();
}

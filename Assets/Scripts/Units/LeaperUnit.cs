using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaperUnit : Monster {

	private bool isLeaping = false;

	private Point leapDestination;

	private Vector3 leapDestinationVector;

	[SerializeField]
	private int leapSpeed;

	void Start () {

		Leaped = false;
	}

	public override UnitDebuff GetUnitDebuff()
	{
		return new DisablerDebuff(Target, DebuffDuration);

	}

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if (GameManager.Instance.WallToLeap != null) {
			
			if (other.name == "WallUnitStopper" && other.transform.parent.parent.GetComponent<TileScript> ().GridPosition == GameManager.Instance.WallToLeap.GridPosition && !StoleBacon) {
				isLeaping = true;

				Stack<Node> newPath = LevelManager.Instance.GeneratePathToLeap (GameManager.Instance.WallToLeap, LevelManager.Instance.RedSpawn);
				SetPath(newPath);
			}
			else if (other.name == "WallUnitStopper" && other.transform.parent.parent.GetComponent<TileScript> ().GridPosition == GameManager.Instance.WallToLeap.GridPosition && StoleBacon) {
				isLeaping = true;

				Stack<Node> newPath = LevelManager.Instance.GeneratePathToLeap (GameManager.Instance.WallToLeap, LevelManager.Instance.BlueSpawn);
				SetPath(newPath);
			}
		}

		base.OnTriggerEnter2D (other);
	}
}
//
//
//
//
//Vector leap attempt...doesn't work nearly as well, sometimes doesn't work at all
//
//	
//
//
//	public void OnTriggerExit2D(Collider2D other)
//	{
//		if(!Leaped)
//		{
//			if (other.gameObject.tag == "Wall" && other.transform.parent.GetComponent<TileScript> () == GameManager.Instance.WallToLeap && !this.StoleBacon) 
//			{
//				this.Speed = this.MaxSpeed;
//				isLeaping = false;
//
//				this.GridPosition = new Point (leapDestination.X, leapDestination.Y);
//
//				Stack<Node> newPath = LevelManager.Instance.GeneratePathToNextPortal (this, LevelManager.Instance.RedSpawn);
//				SetPath (newPath);
//
//				Leaped = true;
//
//			}
//			else if (other.gameObject.tag == "Wall" && other.transform.parent.GetComponent<TileScript> () == GameManager.Instance.WallToLeap && this.StoleBacon) 
//			{
//				this.Speed = this.MaxSpeed;
//				isLeaping = false;
//
//				this.GridPosition = new Point (leapDestination.X, leapDestination.Y);
//	
//				Stack<Node> newPath = LevelManager.Instance.GeneratePathToNextPortal (this, LevelManager.Instance.BlueSpawn);
//				SetPath (newPath);
//
//				Leaped = true;
//				Debug.Log (Leaped);
//
//			}
//		}
//	}
//
//	private void LeapOverWall()
//	{
//		MoveLeaperOverWall();
//	}
//
//	private void DetermineLeapDestination()
//	{
//		Debug.Log ("Unit GP: " + this.GridPosition.X + ", " + this.GridPosition.Y + "\nWall GP: " + GameManager.Instance.WallToLeap.GridPosition.X + ", " + GameManager.Instance.WallToLeap.GridPosition.Y);
//
//		if (GameManager.Instance.WallToLeap.GridPosition.X > this.GridPosition.X) 
//		{
//			if (GameManager.Instance.WallToLeap.GridPosition.Y > this.GridPosition.Y) 
//			{
//				int destX = Mathf.Clamp((GameManager.Instance.WallToLeap.GridPosition.X + 1), 0, 12);
//				int destY = Mathf.Clamp ((GameManager.Instance.WallToLeap.GridPosition.Y + 1), 0, 7);
//
//				leapDestination = LevelManager.Instance.Tiles [new Point (destX, destY)].GridPosition;
//				Debug.Log ("Leap destination X: " + leapDestination.X + "\nLeap destination Y: " + leapDestination.Y);
//
//			} 
//			else 
//			{
//				int destX = Mathf.Clamp((GameManager.Instance.WallToLeap.GridPosition.X + 1), 0, 12);
//				int destY = Mathf.Clamp ((GameManager.Instance.WallToLeap.GridPosition.Y), 0, 7);
//
//				leapDestination = LevelManager.Instance.Tiles [new Point (destX, destY)].GridPosition;
//				Debug.Log ("Leap destination X: " + leapDestination.X + "\nLeap destination Y: " + leapDestination.Y);
//			}
//			leapDestinationVector = new Vector3 (leapDestination.X, leapDestination.Y) - new Vector3(this.GridPosition.X, this.GridPosition.Y);
//
//		}
//		else if (GameManager.Instance.WallToLeap.GridPosition.X < this.GridPosition.X) 
//		{
//			Debug.Log ("X < X");
//
//			if (GameManager.Instance.WallToLeap.GridPosition.Y < this.GridPosition.Y) 
//			{
//				int destX = Mathf.Clamp((GameManager.Instance.WallToLeap.GridPosition.X + 1), 0, 12);
//				int destY = Mathf.Clamp ((GameManager.Instance.WallToLeap.GridPosition.Y - 1), 0, 7);
//
//				leapDestination = LevelManager.Instance.Tiles [new Point (destX, destY)].GridPosition;
//				Debug.Log ("Leap destination X: " + leapDestination.X + "\nLeap destination Y: " + leapDestination.Y);
//				leapDestinationVector = new Vector3(this.GridPosition.X, this.GridPosition.Y) - new Vector3 (leapDestination.X, leapDestination.Y);
//			} 
//			else 
//			{
//				Debug.Log ("Y == Y");
//
//				int destX = Mathf.Clamp((GameManager.Instance.WallToLeap.GridPosition.X - 1), 0, 12);
//				int destY = Mathf.Clamp ((GameManager.Instance.WallToLeap.GridPosition.Y), 0, 7);
//
//				leapDestination = LevelManager.Instance.Tiles [new Point (destX, destY)].GridPosition;
//				Debug.Log ("Leap destination X: " + leapDestination.X + "\nLeap destination Y: " + leapDestination.Y);
//				leapDestinationVector = new Vector3 (leapDestination.X, leapDestination.Y) - new Vector3(this.GridPosition.X, this.GridPosition.Y);
//
//			}
//
//		}
//		else if (GameManager.Instance.WallToLeap.GridPosition.X == this.GridPosition.X) 
//		{
//			Debug.Log ("X == X");
//			if (GameManager.Instance.WallToLeap.GridPosition.Y > this.GridPosition.Y) 
//			{
//				int destX = Mathf.Clamp((GameManager.Instance.WallToLeap.GridPosition.X), 0, 12);
//				int destY = Mathf.Clamp ((GameManager.Instance.WallToLeap.GridPosition.Y + 1), 0, 7);
//
//				leapDestination = LevelManager.Instance.Tiles [new Point (destX, destY)].GridPosition;
//
//				leapDestinationVector = new Vector3(this.GridPosition.X, this.GridPosition.Y) - new Vector3 (leapDestination.X, leapDestination.Y);
//
//				Debug.Log ("Leap destination X: " + leapDestination.X + "\nLeap destination Y: " + leapDestination.Y);
//
//			} 
//			else if (GameManager.Instance.WallToLeap.GridPosition.Y < this.GridPosition.Y)
//			{
//				int destX = Mathf.Clamp((GameManager.Instance.WallToLeap.GridPosition.X), 0, 12);
//				int destY = Mathf.Clamp ((GameManager.Instance.WallToLeap.GridPosition.Y - 1), 0, 7);
//
//				leapDestination = LevelManager.Instance.Tiles [new Point (destX, destY)].GridPosition;
//				Debug.Log ("Leap destination X: " + leapDestination.X + "\nLeap destination Y: " + leapDestination.Y);
//				leapDestinationVector = new Vector3(this.GridPosition.X, this.GridPosition.Y) - new Vector3 (leapDestination.X, leapDestination.Y);
//
//			}
//			else if (GameManager.Instance.WallToLeap.GridPosition.X == this.GridPosition.X) 
//			{
//				int destX = Mathf.Clamp((GameManager.Instance.WallToLeap.GridPosition.X), 0, 12);
//				int destY = Mathf.Clamp ((GameManager.Instance.WallToLeap.GridPosition.Y + 1), 0, 7);
//
//				leapDestination = LevelManager.Instance.Tiles [new Point (destX, destY)].GridPosition;
//				Debug.Log ("Leap destination X: " + leapDestination.X + "\nLeap destination Y: " + leapDestination.Y);
//				leapDestinationVector = new Vector3(this.GridPosition.X, this.GridPosition.Y) - new Vector3 (leapDestination.X, leapDestination.Y);
//			}
//		}
//
//		// - (LevelManager.Instance.TileSize/2)
//
//	}
//
//	private void MoveLeaperOverWall(){
//		if (!StoleBacon) 
//		{
//			transform.Translate (leapDestinationVector * Time.deltaTime);
//		} 
//		else if (StoleBacon) 
//		{
//			transform.Translate (leapDestinationVector * Time.deltaTime);
//		}
//	}
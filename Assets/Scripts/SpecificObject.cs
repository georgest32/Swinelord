using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificObject : SavableObject {

	public override void Start(){
		base.Start ();

	}

	void Update () {
		
	}

	public override void Save (int id)
	{
		base.Save (id);

	}

	public override void Load (string[] values)
	{
		base.Load (values);

		if (this.tag == "Tile") 
		{
			LevelManager.Instance.Tiles.Add(this.GetComponent<TileScript>().GridPosition, this.GetComponent<TileScript>());
			this.transform.SetParent (SaveGameManager.Instance.Map);
		}

		else if (this.tag == "Tower" || this.tag == "Wall") 
		{
			TileScript towerParent = LevelManager.Instance.Tiles [GridPosition];
			this.transform.SetParent (towerParent.transform);
		}

		else if (this.tag == "BluePortal") 
		{
			LevelManager.Instance.BluePortal = this.GetComponent<Portal> ();
		}

		else if (this.tag == "RedPortal") 
		{
			LevelManager.Instance.RedPortal = this.GetComponent<Portal> ();
		}
	}
}



//was in Start()
//
//		if (this.tag == "Tile") {
//			this.GridPosition = GetComponent<TileScript> ().GridPosition;
//			this.WorldPosition = GetComponent<TileScript> ().WorldPosition;
//
//		}
//		else if (this.tag == "Tower") {
//			this.GridPosition = transform.parent.parent.GetComponent<TileScript> ().GridPosition;
//			this.WorldPosition = new Vector2 (transform.parent.GetComponent<TileScript> ().WorldPosition.x - (GetComponent<SpriteRenderer>().bounds.size.x/2), transform.parent.GetComponent<TileScript> ().WorldPosition.y + (GetComponent<SpriteRenderer>().bounds.size.y/2));
//		}
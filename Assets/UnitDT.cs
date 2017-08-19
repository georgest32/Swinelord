using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDT : MonoBehaviour {

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Tile" && !other.GetComponent<TileScript> ().Discovered) 
		{
			this.transform.parent.GetComponent<SpriteRenderer> ().sortingOrder = 1;
			other.GetComponent<TileScript> ().Discovered = true;
			other.GetComponent<TileScript> ().DiscoverTile();
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

enum ObjectType {NONE, MAP, WHITETILE, GREYTILE, WALL, SWINETOWER}

public abstract class SavableObject : MonoBehaviour {

	protected string save;

	[SerializeField]
	private ObjectType objectType;

	private Point gridPosition;

	private Vector2 worldPosition;

	private void Start () 
	{
		SaveGameManager.Instance.SavableObjects.Add (this);

		if (this.tag == "Tile") {
			this.gridPosition = GetComponent<TileScript> ().GridPosition;
			this.worldPosition = GetComponent<TileScript> ().WorldPosition;
		}
		else if (this.tag == "Tower") {
			this.gridPosition = transform.parent.GetComponent<TileScript> ().GridPosition;
			this.worldPosition = new Vector2 (transform.parent.GetComponent<TileScript> ().WorldPosition.x - (GetComponent<SpriteRenderer>().bounds.size.x/2), transform.parent.GetComponent<TileScript> ().WorldPosition.y + (GetComponent<SpriteRenderer>().bounds.size.y/2));
		}
	}

	public virtual void Save(int id)
	{
//		Object prefab = EditorUtility.CreateEmptyPrefab("Assets/Resources/Temporary/"+this.gameObject.name+".prefab");
//		EditorUtility.ReplacePrefab(this.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);

		PlayerPrefs.SetString (id.ToString (), objectType + "_" + transform.position.ToString () + "_" + transform.localScale + "_" + transform.localRotation + "_(" + gridPosition.X + ", " + gridPosition.Y + ")_(" + worldPosition.x + ", " + worldPosition.y + ")");
	}

	public virtual void Load(string[] values)
	{
		Debug.Log (transform.GetComponent<SpriteRenderer> ().sprite.pivot);
		transform.localPosition = SaveGameManager.Instance.StringToVector3(values[1]);
		transform.localScale = SaveGameManager.Instance.StringToVector3(values[2]);
		transform.localRotation = SaveGameManager.Instance.StringToQuaternion(values[3]);

		this.gridPosition = SaveGameManager.Instance.StringToPoint(values[4]);
		this.worldPosition = SaveGameManager.Instance.StringToVector2(values[5]);

		Debug.Log ("gp: " + gridPosition.X + ", " + gridPosition.Y);
		Debug.Log ("wp: " + worldPosition.x + ", " + worldPosition.y);

		if (this.tag == "Tile") {
			this.GetComponent<TileScript> ().Setup (gridPosition, worldPosition, GameManager.Instance.Map.transform);
		}
	}

	public void DestroySavable()
	{

	}
}

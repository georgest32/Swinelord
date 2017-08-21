using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

enum ObjectType {NONE, MAP, BLUEPORTAL, REDPORTAL, WHITETILE, GREYTILE, WALL, SWINETOWER, SPITTERTOWER, TRANSFATTYTOWER, FLATULATIONTOWER, PORKUPINETOWER, THROWBEHINDTOWER, SWINELORDTOWER}

public abstract class SavableObject : MonoBehaviour {

	protected string save;

	[SerializeField]
	private ObjectType objectType;

	public Point GridPosition;

	public Vector2 WorldPosition;

	public virtual void Start () 
	{
		SaveGameManager.Instance.SavableObjects.Add (this);
	}

	public virtual void Save(int id)
	{
//		Object prefab = EditorUtility.CreateEmptyPrefab("Assets/Resources/Temporary/"+this.gameObject.name+".prefab");
//		EditorUtility.ReplacePrefab(this.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
	
		PlayerPrefs.SetString (id.ToString (), objectType + "_" + transform.position.ToString () + "_" + transform.localScale + "_" + transform.localRotation + "_" + GridPosition.X.ToString() + "," + GridPosition.Y.ToString() + "_" + WorldPosition.x + "," + WorldPosition.y);
	}

	public virtual void Load(string[] values)
	{
		transform.localPosition = SaveGameManager.Instance.StringToVector3(values[1]);
		transform.localScale = SaveGameManager.Instance.StringToVector3(values[2]);
		transform.localRotation = SaveGameManager.Instance.StringToQuaternion(values[3]);

		if (this.tag == "Tile") 
		{
			this.GetComponent<TileScript> ().GridPosition = SaveGameManager.Instance.StringToPoint (values [4]);
			this.GetComponent<TileScript> ().WorldPosition = SaveGameManager.Instance.StringToVector2 (values [5]);
		}

		if (this.tag == "Tower" || this.tag == "Wall") 
		{
			transform.GetChild(0).GetComponent<Tower> ().GridPosition = SaveGameManager.Instance.StringToPoint (values [4]);
			transform.GetChild(0).GetComponent<Tower> ().WorldPosition = SaveGameManager.Instance.StringToVector2 (values [5]);
		
			GridPosition = SaveGameManager.Instance.StringToPoint (values [4]);
			WorldPosition = SaveGameManager.Instance.StringToVector2 (values [5]);
		}

		if (this.tag == "BluePortal") 
		{
			LevelManager.Instance.BluePortal = GetComponent<Portal> ();
			LevelManager.Instance.BlueSpawn = SaveGameManager.Instance.StringToPoint (values [4]);
		}

		if (this.tag == "RedPortal") 
		{
			LevelManager.Instance.RedPortal = GetComponent<Portal> ();
			LevelManager.Instance.RedSpawn = SaveGameManager.Instance.StringToPoint (values [4]);
		}
	}

	public void DestroySavable()
	{
		SaveGameManager.Instance.SavableObjects.Remove (this);
	}
}

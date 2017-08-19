using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public class SaveGameManager : MonoBehaviour {

	private static SaveGameManager instance;

	public List<SavableObject> SavableObjects { get; private set; }

	[SerializeField]
	private Transform map;

	public static SaveGameManager Instance 
	{
		get 
		{
			if (instance == null) 
			{
				instance = GameObject.FindObjectOfType<SaveGameManager> ();
			}
			return instance;
		}
	}

	public Transform Map 
	{
		get 
		{
			return map;
		}
		set 
		{
			map = value;
		}
	}

	// Use this for initialization
	void Awake () 
	{
		SavableObjects = new List<SavableObject> ();
	}

	public void Save()
	{
		PlayerPrefs.SetInt ("ObjectCount", SavableObjects.Count);

		for (int i = 0; i < SavableObjects.Count; i++) 
		{
			if (SavableObjects [i].tag == "Wall" || SavableObjects [i].tag == "Tower") 
			{
				Debug.Log (SavableObjects [i].transform.GetChild (0).GetComponent<Tower> ().GridPosition.X);
				SavableObjects [i].GridPosition = SavableObjects [i].transform.GetChild(0).GetComponent<Tower> ().GridPosition;
			}
			else if (SavableObjects [i].tag == "Tile") 
			{
				SavableObjects [i].GridPosition = SavableObjects [i].GetComponent<TileScript> ().GridPosition;
			}

			SavableObjects [i].Save (i);
		}
	}

	public void Load()
	{
		foreach (SavableObject obj in SavableObjects) 
		{
			if (obj != null) 
			{
				Destroy(obj.gameObject);
			}
		}

		SavableObjects.Clear ();

		LevelManager.Instance.Tiles.Clear ();

		int objectCount = PlayerPrefs.GetInt ("ObjectCount");

		for (int i = 0; i < objectCount; i++) 
		{	
			string[] value = PlayerPrefs.GetString (i.ToString ()).Split('_');
			GameObject tmp = null;
			switch (value [0]) {
			case "MAP":
				tmp = Instantiate(Resources.Load("Prefabs/Map") as GameObject);
				break;
			case "WHITETILE":
				tmp = Instantiate(Resources.Load("Prefabs/WhiteTile") as GameObject);
				break;
			case "GREYTILE":
				tmp = Instantiate(Resources.Load("Prefabs/GreyTile") as GameObject);
				break;
			case "WALL":
				tmp = Instantiate(Resources.Load("Prefabs/Towers/Wall") as GameObject);
				break;
			case "SWINETOWER":
				tmp = Instantiate(Resources.Load("Prefabs/Towers/SwineTower") as GameObject);
				break;
			}

			if (tmp != null) 
			{
				tmp.GetComponent<SavableObject> ().Load (value);
			}
		}

		AStar.CreateNodes ();
	}

	public Vector3 StringToVector3(string value)
	{
		//(1, 24, 4)
		value = value.Trim (new char[] {'(', ')'});

		//1, 24, 4
		value = value.Replace (" ", "");

		//1,24,4
		string[] pos = value.Split (',');

		//[0] = 1 [1] = 24 [2] = 4
		return new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
	}

	public Vector2 StringToVector2(string value)
	{
		//(1, 24, 4)
		value = value.Trim (new char[] {'(', ')'});

		//1, 24, 4
		value = value.Replace (" ", "");

		//1,24,4
		string[] pos = value.Split (',');

		//[0] = 1 [1] = 24 [2] = 4
		return new Vector2(float.Parse(pos[0]), float.Parse(pos[1]));
	}

	public Quaternion StringToQuaternion(string value)
	{
		//(1, 24, 4)
		value = value.Trim (new char[] {'(', ')'});

		//1, 24, 4
		value = value.Replace (" ", "");

		//1,24,4
		string[] pos = value.Split (',');

		//[0] = 1 [1] = 24 [2] = 4
		return new Quaternion(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]), float.Parse(pos[3]));
	}

	public Point StringToPoint(string value)
	{
		//(1, 24, 4)
		value = value.Trim (new char[] {'(', ')'});

		//1, 24, 4
		value = value.Replace (" ", "");

		//1,24,4
		string[] pos = value.Split (',');
		//[0] = 1 [1] = 24 [2] = 4

		int pointX = Convert.ToInt32 (float.Parse (pos [0]));
		int pointY = Convert.ToInt32 (float.Parse (pos [1]));

		Point newPoint = new Point (pointX, pointY);
//		Debug.Log (newPoint.X + ", " + newPoint.Y);

//		transform.GetChild(0).GetComponent<Tower> ().GridPosition = SaveGameManager.Instance.StringToPoint (values [4]);

		return newPoint;
	}
}

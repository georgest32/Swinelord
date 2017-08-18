//using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

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

		for (int i = 0; i < SavableObjects.Count; i++) {
			SavableObjects [i].Save (i);
		}
	}

	public void Load()
	{
		LevelManager.Instance.Tiles.Clear ();

		foreach (SavableObject obj in SavableObjects) {
			if (obj != null) {
				Destroy(obj.gameObject);
			}
		}

		SavableObjects.Clear ();

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
		return new Point((int)float.Parse(pos[0]), (int)float.Parse(pos[1]));
	}
}

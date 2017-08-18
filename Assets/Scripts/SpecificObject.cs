using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificObject : SavableObject {

	private Point gridPosition;

	private int level;
	
	void Update () {
		
	}

	public override void Save (int id)
	{
		base.Save (id);
	}

	public override void Load (string[] values)
	{
		base.Load (values);

//		if (this.tag == "Tile") {
//			this.transform.SetParent (SaveGameManager.Instance.Map);
//		}
	}
}

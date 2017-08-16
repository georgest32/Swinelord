using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Portal : MonoBehaviour {

	private Animator myAnimator;

	private GameObject selectedPortal;

	[SerializeField]
	private GameObject portalPrefab;

	public GameObject PortalPrefab { get{ return portalPrefab; } }

	// Use this for initialization
	void Start () {
		myAnimator = GetComponent<Animator> ();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			SelectPortal ();
		}
	}
	
	// Update is called once per frame
	public void Open () {
		myAnimator.SetTrigger ("Open");
	}

	public void SelectPortal()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

		RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

		if (!EventSystem.current.IsPointerOverGameObject () && hit != null && hit.transform.gameObject.GetComponent<Portal>()) {
			GameManager.Instance.SelectedPortal = hit.transform.gameObject;
			Hover.Instance.Activate (GameManager.Instance.SelectedPortal.GetComponent<SpriteRenderer> ().sprite);
			GameManager.Instance.ChangingPortal = true;
			GameManager.Instance.SelectedPortal.SetActive(false);
		}
	}

	public void Destroy()
	{
		selectedPortal = null;
		Destroy (gameObject);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {

	private Animator myAnimator;

	private GameObject selectedPortal;

	[SerializeField]
	private GameObject portalPrefab;

	public GameObject PortalPrefab { get{ return portalPrefab; } }

	[SerializeField]
	private bool discovered;

	void Start () {
		myAnimator = GetComponent<Animator> ();

		if (!discovered && SceneManager.GetActiveScene().name != "Home") 
		{
			GetComponent<SpriteRenderer> ().enabled = false;
		} 
		else if (discovered) 
		{
			GetComponent<SpriteRenderer> ().enabled = true;
		}
	}

	void Update()
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			SelectPortal ();
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "DiscoverTrigger") 
		{
			GetComponent<SpriteRenderer> ().enabled = true;
			discovered = true;
		}
	}
	
	public void Open () {
		myAnimator.SetTrigger ("Open");
	}

	public void SelectPortal()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

		RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

		if (!EventSystem.current.IsPointerOverGameObject () && hit != null && hit.transform.gameObject.GetComponent<Portal>()) 
		{
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

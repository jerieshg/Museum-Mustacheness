using UnityEngine;
using System.Collections;

public class PlayerCastController : MonoBehaviour {

	public GameObject playerCastPosition;
	public GameObject marker;

	private bool canCast;

	void Start () {
	
	}
	
	void Update () {
		cast ();
	}

	private void cast()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			marker.GetComponent<Marker> ().direction = transform.right * transform.localScale.x;
			Instantiate (marker,playerCastPosition.transform.position,playerCastPosition.transform.rotation);
		}
	}
}

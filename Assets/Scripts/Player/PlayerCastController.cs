using UnityEngine;
using System.Collections;
using System;

public class PlayerCastController : MonoBehaviour {

	public GameObject playerCastPosition;
	public GameObject marker;
	public bool throwing = false;
	
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			throwing = true;
		}
	}

	private void throwMarker()
    {
		marker.GetComponent<Marker> ().direction = transform.right * transform.localScale.x;
		Instantiate (marker,playerCastPosition.transform.position,playerCastPosition.transform.rotation);
	}

    private void cancelThrow()
    {
        throwing = false;
    }
}

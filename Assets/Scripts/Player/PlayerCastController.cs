using UnityEngine;
using System.Collections;
using System;

public class PlayerCastController : MonoBehaviour {

	public GameObject playerCastPosition;
	public GameObject marker;
	public bool throwing = false;
	
	void Update () {
		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if (Input.GetButtonDown ("Fire1")) {
			throwing = true;
		}
		#endif
	}

	private void throwMarker()
    {
		marker.GetComponent<Marker> ().direction = transform.right * transform.localScale.x;
		Instantiate (marker,playerCastPosition.transform.position,playerCastPosition.transform.rotation);
	}

	public void isThrowing(){
		throwing = true;
	}

    public void cancelThrow()
    {
        throwing = false;
    }
}

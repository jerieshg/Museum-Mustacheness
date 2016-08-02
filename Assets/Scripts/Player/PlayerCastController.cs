using UnityEngine;
using System.Collections;
using System;

public class PlayerCastController : MonoBehaviour {
	
	[Header("Cast Position & Throwable Item")]
	public GameObject playerCastPosition;
	public GameObject marker;

	[HideInInspector] public bool throwing = false;

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

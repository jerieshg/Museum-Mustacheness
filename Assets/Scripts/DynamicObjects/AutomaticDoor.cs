using UnityEngine;
using System.Collections;

public class AutomaticDoor : MonoBehaviour {

	private Animator doorAnim;

	void Start()
	{
		doorAnim = GetComponent<Animator> ();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Player" || col.tag == "Mob" || col.tag == "Enemy")
		{
			doorAnim.SetBool ("On", true);
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.tag == "Player" || col.tag == "Mob" || col.tag == "Enemy")
		{
			doorAnim.SetBool ("On", false);
		}
	}

}

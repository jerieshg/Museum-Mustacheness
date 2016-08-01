using UnityEngine;
using System.Collections;

public class CameraVision : MonoBehaviour
{
	
	public GameObject alarm;
	protected Animator alarmAnim;
	protected GameObject vision;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			alarmAnim = alarm.GetComponent<Animator> ();
			alarmAnim.SetInteger ("active", 1);
		}
	}
}

using UnityEngine;
using System.Collections;
using Microsoft.Win32;

public class CharacterController : MonoBehaviour {

	public Stats referenceStats;
	[HideInInspector] public Animator animator;

	void Awake(){
		animator = GetComponent<Animator> ();
	}

	public IEnumerator takeDamage(int damage){
		//change to take damage anim
//		referenceStats.hitpoints--;
		animator.SetTrigger ("hit");
		yield return new WaitForSeconds (0.05f);
		//change to regular anim
	}

}
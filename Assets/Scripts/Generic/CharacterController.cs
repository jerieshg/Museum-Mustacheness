using UnityEngine;
using System.Collections;
using Microsoft.Win32;

public class CharacterController : MonoBehaviour {

	public Stats referenceStats;

	void LateUpdate(){
		OnDeath ();
	}

	public IEnumerator takeDamage(int damage){
		//change to take damage anim
		yield return new WaitForSeconds (0.05f);
		//change to regular anim
		referenceStats.hitpoints--;
	}

	private void OnDeath(){
		if (referenceStats.hitpoints < 1) {
			//Execute death anim
		}
	}
}

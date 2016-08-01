using UnityEngine;
using System.Collections;

public class CameraAI : MonoBehaviour
{
	protected Animator animator;
	protected Transform vision;
	protected Animation anim;
	
	int movR = 1;
	int movL = 1;
	public int div = 5;
	int countTest = 0;
	bool pass = true;
	
	void Start ()
	{
		animator = GetComponent<Animator> ();		
		vision = this.gameObject.transform.GetChild (0);
	}
	
	void Update ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("rightIdle")) {
			vision.position = new Vector3 (transform.position.x + 2, transform.position.y - 2, transform.position.z);
		}
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("leftIdle")) {
			vision.position = new Vector3 (transform.position.x - 2, transform.position.y - 2, transform.position.z);
		}
		
		if (div < 2)
			div = 2;
		int letime = (int)Time.time;
		
		int boolo = letime % div;

		if (boolo == 0) {
			if (pass) {
				Debug.Log (countTest);
				countTest++;

				if (movR > 0) {			
					movR = 3;
					animator.SetInteger ("MovR", movR);
					animator.SetInteger ("MovL", movL);
				
					movR = 0;
					movL = 3;			
				
				} else {
					animator.SetInteger ("MovL", movL);
					animator.SetInteger ("MovR", movR);
				
					movL = 0;
					movR = 3;
				
				}
				pass = false;
			}
		} else
			pass = true;
	}
}
using UnityEngine;
using System.Collections;

public class camaraAI : MonoBehaviour {
	protected Animator animator;
	protected Transform vision;
	protected Animation anim;
	
	int movR = 1;
	int movL = 1;
	public int div = 5;
	int countTest = 0;
	bool pass = true;
	

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();		
		vision = this.gameObject.transform.GetChild(0);
		

	}
	
	// Update is called once per frame
	void Update () {
		
		if(div < 2)
			div = 2;
		int letime = (int)Time.time;
		
		int boolo = letime % div;

		if(boolo == 0){
			if(pass){
			Debug.Log(countTest);
			countTest++;


			if (movR > 0){					
				vision.position = new Vector3(vision.position.x + 4,vision.position.y,vision.position.z);				
				movR = 3;
				animator.SetInteger("MovR", movR);
				animator.SetInteger("MovL", movL);
				movR = 0;
				movL = 3;			
				
			}
			else {
				vision.position = new Vector3(vision.position.x - 4,vision.position.y,vision.position.z);				
				animator.SetInteger("MovL", movL);
				animator.SetInteger("MovR", movR);
				movL = 0;
				movR = 3;
				
			}

			pass = false;
			}
			/**/

				
			
		}
		else
			pass = true;

		
		

		

	}
}

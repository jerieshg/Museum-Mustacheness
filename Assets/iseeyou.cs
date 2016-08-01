using UnityEngine;
using System.Collections;

public class iseeyou : MonoBehaviour {
	public GameObject alarm;	
	protected Animator alarmanim;
	protected GameObject vision;

	// Use this for initialization
	void Start () {
		
	
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") 
         {
         	//alarm = this.gameObject.parent.GetChild(1);
         	alarmanim = alarm.GetComponent<Animator>();
         	alarmanim.SetInteger("active", 1);


         }
		
	}
}

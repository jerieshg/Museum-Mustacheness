using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.IO;
using UnityEditor;

public class Marker : MonoBehaviour {

	public float markerCooldown = 2f;
	public float markerSpeed = 5f;
	public float markerRadiusImpact = 0.2f;
	public LayerMask hitMask;
	public LayerMask collision;

	[HideInInspector] public Vector3 direction;

	private float lifetime = 5f;

	void Awake () {
		Destroy(this.gameObject, lifetime);
	}

	
	void Update () {
		travelDirection ();
		checkCollisions ();
	}

	//Checks the collision type, depending on which collision it will interact differently
	void checkCollisions()
	{
		RaycastHit2D hitMob = Physics2D.CircleCast (transform.position, markerRadiusImpact, new Vector2 (0.5f, 0.5f), 0.1f,hitMask);
		RaycastHit2D hitCol = Physics2D.CircleCast (transform.position, markerRadiusImpact, new Vector2 (0.5f, 0.5f), 0.1f, collision);

		if(hitMob)
		{
			if(hitMob.transform.tag == "Player" || hitMob.transform.tag == "Mob")
			{
				//Dosomething
				Destroy(gameObject);
			}
		}

		if(hitCol)
		{
			
			Destroy(gameObject);
		}
	}

	private void travelDirection(){
		if (direction != null) {
			transform.Translate (direction * Time.fixedDeltaTime * markerSpeed);
		}
	}
}

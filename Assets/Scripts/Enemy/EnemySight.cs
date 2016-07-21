using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {

	public float fovAngle = 180f;
	public bool playerInSight;
	public Vector3 personalLastSighting; 
	public LayerMask layerMask;

	private CircleCollider2D circle;
	private LastPlayerSighting lastPlayerSighting;
	private GameObject player;
	private Vector3 previousSighting;

	void Awake(){
		circle = GetComponent<CircleCollider2D> ();
		lastPlayerSighting = GameObject.FindGameObjectWithTag(Tag.GameController.ToString()).GetComponent<LastPlayerSighting>();
		player = GameObject.FindGameObjectWithTag (Tag.Player.ToString ());

		personalLastSighting = lastPlayerSighting.resetPosition;
		previousSighting = lastPlayerSighting.resetPosition;
	}
	
	// Update is called once per frame
	void Update () {
		// If the last global sighting of the player has changed...
		if (lastPlayerSighting.position != previousSighting) {
			// ... then update the personal sighting to be the same as the global sighting.
			previousSighting = lastPlayerSighting.position;

			//IfPlayerIsAliveDoSomething
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject == player) {
			
			playerInSight = false;

			//Create a vector from the enemy to the player and store the angle between it and forward.
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle (direction, (transform.position.x > 0) ? transform.right : -transform.right);

			if (angle <= (fovAngle )) {
				RaycastHit2D hit;

				if(hit = Physics2D.Raycast (transform.position, direction.normalized, circle.radius)){
					
					if (hit.collider.gameObject == player) {
						playerInSight = true;
						lastPlayerSighting.position = player.transform.position;
						personalLastSighting = player.transform.position;
					}
				}
			}

		}
	}

	void OnTriggerExit (Collider other)	{
		// If the player leaves the trigger zone...
		if(other.gameObject == player)
			// ... the player is not in sight.
			playerInSight = false;
	}
}

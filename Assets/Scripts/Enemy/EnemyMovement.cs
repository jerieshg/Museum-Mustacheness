using UnityEngine;
using System.Collections;
using System;

public class EnemyMovement : MonoBehaviour {

	public float patrolSpeed = 4f;
	public float maxPatrolSpeed = 3f;
	public float chaseSpeed = 6f;
	public float maxChaseSpeed = 5f;

	public float jumpForce;
	public float jumpPushForce = 1;


	public LayerMask playerMask;
	public LayerMask collisions; 

	private LastPlayerSighting lastPlayerSighting; 
	private EnemySight enemySight;
	private Rigidbody2D rigidBody;
	private Vector3 originalPosition;
	private bool isGoingLeft;
	private bool isGround;
	private bool isWall;
	private float chaseTimer;     
	public float chaseWaitTime = 5f;   
	const float distanceToCollision = 0.8f;

	void Start () {
		originalPosition = this.transform.localPosition;
		rigidBody = GetComponent<Rigidbody2D> ();
		lastPlayerSighting = GameObject.FindGameObjectWithTag(Tag.GameController.ToString()).GetComponent<LastPlayerSighting>();
		enemySight = GetComponent<EnemySight>();
	}

	void Update () {

		float move = rigidBody.velocity.x;

		 if (enemySight.personalLastSighting != lastPlayerSighting.resetPosition) {
			chasing ();
		} else {
			patrol ();
		}

		if (isWall) {
			if (isGround) {
				rigidBody.AddForce (new Vector2 (jumpPushForce, jumpForce));
			}
		} 


		if (move > 0 && isGoingLeft) {
			flip ();
		} else if (move < 0 && !isGoingLeft) {
			flip ();
		}

	}

	void FixedUpdate(){

		checkEnemySurroundings ();
	}

	private void move(Vector2 direction, float speed, float maxSpeed, Boolean xAxisOnly){
		if (Mathf.Abs (rigidBody.velocity.x) < maxSpeed) {
			if (xAxisOnly) {
				rigidBody.velocity = new Vector2(rigidBody.velocity.x + direction.x * speed * Time.deltaTime, rigidBody.velocity.y);
			} else {
				rigidBody.velocity += direction * speed * Time.deltaTime;
			}

		}
	}

	private void patrol(){
		Vector2 direction = (isGoingLeft) ? Vector2.left : Vector2.right;
		move (direction, patrolSpeed, maxPatrolSpeed, false);
	}

	private void chasing(){
		Vector3 sightingPosition = enemySight.personalLastSighting - transform.position;

		if (sightingPosition.sqrMagnitude > 6f && enemySight.playerInSight) {
			move (sightingPosition, chaseSpeed, maxChaseSpeed, true);
		} else {
			chaseTimer += Time.deltaTime;

			// If the timer exceeds the wait time...
			if(chaseTimer >= chaseWaitTime)
			{
				// ... reset last global sighting, the last personal sighting and the timer.
				lastPlayerSighting.position = lastPlayerSighting.resetPosition;
				enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
				chaseTimer = 0f;
			}
		}
			
	}

	private void flip(){
		// Switch the way the player is labelled as facing
		isGoingLeft = !isGoingLeft;

		//Multiply the player's x local cale by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	//NEEDS REVIEW

	private void checkEnemySurroundings ()
	{
		isGround = false;
		isWall = false;

		RaycastHit2D hit = Physics2D.Raycast (transform.position, -Vector2.up, 2f, collisions);
		if (hit.collider != null && hit.distance< distanceToCollision) {
			isGround = true;
		}

		Vector2 direction = (isGoingLeft) ? Vector2.left : Vector2.right;

		hit = Physics2D.Raycast (transform.position, direction, 5f, collisions);

		if ( hit.collider != null && hit.distance < distanceToCollision){
			print ( Physics2D.gravity * jumpForce * Time.deltaTime);
			isWall = true;
		}
	}


}

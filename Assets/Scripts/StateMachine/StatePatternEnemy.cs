using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Runtime.InteropServices;

public class StatePatternEnemy : MonoBehaviour {
	
	public float patrolSpeed = 4f;
	public float chaseSpeed = 6f;
	public float jumpForce = 40f;
	public float searchingTurnSpeed = 120f;
	public float searchingDuration = 4f;
	public float sightRange = 20f;
	public float maxDistance = 10f;
	public Transform eyes;
	public Vector3 offset = new Vector3(0, 0.5f, 0);
	public LayerMask collisions; 

	[HideInInspector] public Transform chaseTarget;
	[HideInInspector] public bool isGoingLeft;
	[HideInInspector] public IEnemyState currentState;
	[HideInInspector] public ChaseState chaseState;
	[HideInInspector] public AlertState alertState;
	[HideInInspector] public PatrolState patrolState;
	[HideInInspector] public Rigidbody2D rigidBody;
	[HideInInspector] public MeshRenderer meshRenderer;
	[HideInInspector] public bool exceededDistance;
	[HideInInspector] public bool resettingPosition;
	[HideInInspector] public Vector3 startPosition;

	const float distanceToCollision = 0.8f;
	private bool wallDetected;
	private bool grounded;



	void Awake(){
		chaseState = new ChaseState (this);
		alertState = new AlertState (this);
		patrolState = new PatrolState (this);
		rigidBody = GetComponent<Rigidbody2D> ();
		startPosition = transform.position;
		meshRenderer = GetComponent<MeshRenderer> ();//debug
	}

	void Start () {
		currentState = patrolState;
	}
	
	void Update () {
		exceededDistance = (retrieveDistanceFromStartPosition() > maxDistance);
	
		if (!resettingPosition) {
			currentState.UpdateState ();
		} else {
			
			move (startPosition, patrolSpeed);
			resettingPosition = (retrieveDistanceFromStartPosition() > 0.3f);
		}

		if (wallDetected) {
			if (grounded) {
				rigidBody.AddForce (new Vector2 (0f, jumpForce));
			}
		} 
	}

	void OnTriggerEnter2D (Collider2D other){
		currentState.OnTriggerEnter2D (other);
	}

	public void move(float speed){
		transform.Translate (speed * Time.fixedDeltaTime * transform.localScale.x, 0, 0);
	}

	public void move(Vector3 target, float speed){
		transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.x, transform.position.y), speed * Time.deltaTime);
	}

	public void flip(){
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	/**
	 * Used to manage the A.I Jumping 
	 **/
	private void checkEnemySurroundings ()	{
		grounded = false;
		wallDetected = false;

		RaycastHit2D hit = Physics2D.Raycast (transform.position, -Vector2.up, 2f, collisions);
		if (hit.collider != null && hit.distance< distanceToCollision) {
			grounded = true;
		}

		Vector2 direction = (isGoingLeft) ? Vector2.left : Vector2.right;

		hit = Physics2D.Raycast (transform.position, direction, 5f, collisions);

		if ( hit.collider != null && hit.distance < distanceToCollision){
			wallDetected = true;
		}
	}

	private float retrieveDistanceFromStartPosition(){
		return Vector3.Distance (startPosition, transform.position);
	}

}

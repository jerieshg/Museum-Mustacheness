using UnityEngine;
using System.Collections;
using UnityEditor;

public class StatePatternEnemy : MonoBehaviour {
	
	public float patrolSpeed = 4f;
	public float maxPatrolSpeed = 3f;
	public float chaseSpeed = 6f;
	public float maxChaseSpeed = 5f;
	public float jumpForce = 40f;
	public float searchingTurnSpeed = 120f;
	public float searchingDuration = 4f;
	public float sightRange = 20f;
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

	const float distanceToCollision = 0.8f;
	private bool wallDetected;
	private bool grounded;



	void Awake(){
		chaseState = new ChaseState (this);
		alertState = new AlertState (this);
		patrolState = new PatrolState (this);
		rigidBody = GetComponent<Rigidbody2D> ();
		meshRenderer = GetComponent<MeshRenderer> ();//debug
	}

	// Use this for initialization
	void Start () {
		currentState = patrolState;
	}
	
	// Update is called once per frame
	void Update () {
		checkEnemySurroundings ();
		float move = rigidBody.velocity.x;

		if (move > 0 && isGoingLeft) {
			flip ();
		} else if (move < 0 && !isGoingLeft) {
			flip ();
		}

		currentState.UpdateState ();

		if (wallDetected) {
			if (grounded) {
				rigidBody.AddForce (new Vector2 (0f, jumpForce));
			}
		} 
	}

	void OnTriggerEnter2D (Collider2D other){
		currentState.OnTriggerEnter2D (other);
	}

	public void move(Vector2 direction, float speed, float maxSpeed, bool xAxisOnly){
		if (Mathf.Abs (rigidBody.velocity.x) < maxSpeed) {
			if (xAxisOnly) {
				rigidBody.velocity = new Vector2(rigidBody.velocity.x + direction.x * speed * Time.deltaTime, rigidBody.velocity.y);
			} else {
				rigidBody.velocity += direction * speed * Time.deltaTime;
			}

		}
	}

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

	private void flip(){
		// Switch the way the player is labelled as facing
		isGoingLeft = !isGoingLeft;

		//Multiply the player's x local cale by -1
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}

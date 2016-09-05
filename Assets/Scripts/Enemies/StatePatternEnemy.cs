using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class StatePatternEnemy : CharacterController {


	[Header("Movement Variables")]
	public float patrolSpeed = 4f;
	public float chaseSpeed = 6f;
	public float jumpForce = 40f;

	[Header("Patrol & Alert State Variables")]
	public float stateChangeDuration = 4f;
	public float searchingTurnSpeed = 120f;
	public float searchingDuration = 4f;
	public float searchingRotateDuration = 2f;
	public float sightRange = 20f;
	public float maxDistance = 10f;
	public float stoppingDistance = 0.4f;

	[Header("Chase State Variables")]
	public GameObject enemyCastPosition;
	public GameObject bullet;
	public float shootingDistance = 4f;

	[Header("Patrolling Waypoints")]
	public Transform[] waypoints;

	[Header("Additional Mob Info")]
	public Transform eyes;
	public Vector3 offset = new Vector3(0, 0.5f, 0);

	[Header("Layers")]
	public LayerMask collisions; 
	public LayerMask playerLayer; 

	[Header("Debug")]
	public bool forceChaseState;

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
	[HideInInspector] public RaycastHit2D hitInfo;
	[HideInInspector] public EnemyAnimationController animationController;
	[HideInInspector] public RaycastHit2D eyesHitInfo;

	const float distanceToCollision = 0.8f;
	private bool wallDetected;
	private bool grounded;
	private float shootingCooldown;

	void Awake(){
		chaseState = new ChaseState (this);
		alertState = new AlertState (this);
		patrolState = new PatrolState (this);
		rigidBody = GetComponent<Rigidbody2D> ();
		startPosition = transform.position;
		animationController = GetComponent<EnemyAnimationController> ();
		this.referenceStats = GetComponent<MobStats> ();
	}

	void Start () {
		currentState = patrolState;
	}
	
	void Update () {
		if (forceChaseState) {
			currentState = chaseState;
			chaseTarget = waypoints [1];
		}

		eyes.GetComponent<DrawLine> ().XLineDirection = sightRange;


		exceededDistance = (retrieveDistanceFromStartPosition() > maxDistance);
		isGoingLeft = (transform.localScale.x >= 0) ? false : true;
		hitInfo = GetComponent<DrawCircle> ().hitInfo;
		eyesHitInfo = eyes.GetComponent<DrawLine> ().hitInfo;

		if (!resettingPosition) {
			currentState.UpdateState ();
		} else {
			move (startPosition, patrolSpeed);
			resettingPosition = (retrieveDistanceFromStartPosition() > stoppingDistance);
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

	public void Shoot(){
		bullet.GetComponent<Projectile> ().direction = transform.right * transform.localScale.x;
		Instantiate (bullet, enemyCastPosition.transform.position, enemyCastPosition.transform.rotation);
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
		print (direction);

		hit = Physics2D.Raycast (transform.position, direction, 5f, collisions);

		if ( hit.collider != null && hit.distance < distanceToCollision){
			wallDetected = true;
		}
	}

	public void correctDirection(Vector3 targetPosition){
//		if (chaseTarget != null) {
			Vector3 distance = targetPosition - transform.position;
			turnSprite ((distance.x >= 0) ? 1 : -1);
//		}
	}

	public void turnSprite(float x){
		Vector3 scale = transform.localScale;
		scale.x = x;
		transform.localScale = scale;
	}

	public void turnSprite(){
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	private float retrieveDistanceFromStartPosition(){
		return Vector3.Distance (startPosition, transform.position);
	}
}

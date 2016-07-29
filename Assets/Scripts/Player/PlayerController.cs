using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	//Controllers
	private PlayerAnimationController playerAnimationController;
	private PlayerMovementController playerMovementController;

	//Movement
	public float maxSpeed;
	public float jumpForce;
	public float jumpPushForce = 10f;
	public LayerMask collisions;

	[HideInInspector] public Rigidbody2D rigidBody;

	private float attackTimeCd = 0f;
	private bool canAttack;

	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		playerAnimationController = GetComponent<PlayerAnimationController>();
		playerMovementController = new PlayerMovementController(this);

	}
	
	void Update () {
		playerMovementController.checkJump ();
	}

	void FixedUpdate(){
		checkPlayerSurroundings ();
		handleAnimations ();
		playerMovementController.move ();
	}

	private void checkPlayerSurroundings ()
	{
		playerMovementController.isGround = false;
		playerMovementController.isWall = false;

		RaycastHit2D hit = Physics2D.Raycast (transform.position, -Vector2.up, 2f, collisions);
	
		if (hit.collider != null && hit.distance< playerMovementController.distanceToCollision) {
			playerMovementController.isGround = true;
		}

		Vector2 direction = (playerMovementController.isLeft) ? Vector2.left : Vector2.right;

		hit = Physics2D.Raycast (transform.position, direction, 5f, collisions);

		if ( hit.collider != null && hit.distance < playerMovementController.distanceToCollision){
			playerMovementController.isWall = true;
		}
	}

	private void handleAnimations(){
		playerAnimationController.walk = playerMovementController.walking;
		playerAnimationController.jump = !playerMovementController.isGround;
	}
}

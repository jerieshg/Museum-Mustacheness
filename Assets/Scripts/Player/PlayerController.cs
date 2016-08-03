using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	[Header("Layers")]
	public LayerMask collisions;

	[Header("Player Stats Variables")]
	public float hitpoints;
	public int markers;
	public int score;

	[Header("Player Movement Variables")]
	public float maxSpeed;
	public float jumpForce;
	public float jumpPushForce = 10f;

	[Header("Cast Position & Throwable Item")]
	public GameObject playerCastPosition;
	public GameObject marker;

	[HideInInspector] public Rigidbody2D rigidBody;

	//Controllers
	private PlayerAnimationController playerAnimationController;
	private PlayerMovementController playerMovementController;
	private PlayerStats playerStats;

	//Other variables
	private float attackTimeCd = 0f;
	private bool throwing;
	private bool canAttack;

	void Start ()
	{
		playerStats = new PlayerStats (hitpoints, markers, score);
		rigidBody = GetComponent<Rigidbody2D> ();
		playerAnimationController = GetComponent<PlayerAnimationController> ();
		playerMovementController = new PlayerMovementController (this);
	}

	void Update ()
	{
		playerMovementController.checkJump ();
	}

	void FixedUpdate ()
	{
		checkPlayerSurroundings ();
		handleAnimations ();
		playerMovementController.move ();
	}

	public void isThrowing(){
		throwing = true;
	}

	public void cancelThrow()
	{
		throwing = false;
	}

	public void moveLeft(){
		playerMovementController.movingLeft = true;
	}

	public void cancelMoveLeft(){
		playerMovementController.movingLeft = false;
	}

	public void moveRight(){
		playerMovementController.movingRight = true;
	}

	public void cancelMoveRight(){
		playerMovementController.movingRight = false;
	}

	public void jump(){
		playerMovementController.canJump = true;
	}

	private void throwMarker()
	{
		marker.GetComponent<Marker> ().direction = transform.right * transform.localScale.x;
		Instantiate (marker,playerCastPosition.transform.position, playerCastPosition.transform.rotation);
	}

	private void checkPlayerSurroundings ()
	{
		playerMovementController.isGround = false;
		playerMovementController.isWall = false;

		RaycastHit2D hit = Physics2D.Raycast (transform.position, -Vector2.up, 2f, collisions);
	
		if (hit.collider != null && hit.distance < playerMovementController.distanceToCollision) {
			playerMovementController.isGround = true;
		}

		Vector2 direction = (playerMovementController.isLeft) ? Vector2.left : Vector2.right;

		hit = Physics2D.Raycast (transform.position, direction, 5f, collisions);

		if (hit.collider != null && hit.distance < playerMovementController.distanceToCollision) {
			playerMovementController.isWall = true;
		}
	}

	private void handleAnimations ()
	{
		playerAnimationController.walk = playerMovementController.walking;
		playerAnimationController.jump = !playerMovementController.isGround;
		playerAnimationController.throwMarker = throwing;
	}
}

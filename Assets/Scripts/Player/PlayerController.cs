using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	[Header ("Layers")]
	public LayerMask collisions;

	[Header ("Player Stats Variables")]
	public float hitpoints;
	public int markers;
	public int score;

	[Header ("Player Movement Variables")]
	public float maxSpeed;
	public float jumpForce;
	public float jumpPushForce = 10f;
	public float distanceToCollision = 0.5f;
	public float wallPushForce = 10f;

	[Header ("Transforms Position & Throwable Item")]
	public GameObject playerCastPosition;
	public Transform[] playerGroundedPositions;
	public GameObject marker;

	[HideInInspector] public Rigidbody2D rigidBody;

	//Controllers
	private PlayerAnimationController playerAnimationController;
	private PlayerMovementController playerMovementController;
	private PlayerStats playerStats;

	//Other variables
	private float attackTimeCd = 0f;
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
		checkPlayerSurroundings ();
	}

	void FixedUpdate ()
	{
		handleAnimations ();
		playerMovementController.move ();
	}

	public void isThrowing ()
	{
		playerAnimationController.throwMarker = true;
	}

	public void cancelThrow ()
	{
		playerAnimationController.throwMarker = false;
	}

	public void cancelHit(){
		playerAnimationController.hit = false;
	}

	public PlayerMovementController getPlayerMovementController ()
	{
		return this.playerMovementController;
	}

	public int getScore()
	{
		return this.playerStats.score;
	}

	public void setScore(int sc)
	{
		this.playerStats.score = sc;

		if(this.playerStats.score <= 0)
		{
			this.playerStats.score = 0;
		}
	}

	public void setMarkers(int mkr)
	{
		this.playerStats.markers = mkr;

		if(this.playerStats.markers <= 0)
		{
			this.playerStats.markers = 0;
		}
	}

	public int getMarkers()
	{
		return this.playerStats.markers;
	}

	private void throwMarker ()
	{
		if (playerStats.markers != 0) {
			marker.GetComponent<Projectile> ().direction = transform.right * transform.localScale.x;
			Instantiate (marker, playerCastPosition.transform.position, playerCastPosition.transform.rotation);
			playerStats.markers--;
		}
	}

	private void checkPlayerSurroundings ()
	{
		playerMovementController.isGround = false;
		playerMovementController.isWall = false;

		foreach (Transform e in playerGroundedPositions) {
			RaycastHit2D hit = Physics2D.Raycast (e.position, -Vector2.up, 2f, collisions);
			if (hit.collider != null && hit.distance < distanceToCollision) {
				playerMovementController.isGround = true;
			}
		}

		Vector2 direction = (playerMovementController.isLeft) ? Vector2.left : Vector2.right;

		RaycastHit2D wallHit = Physics2D.Raycast (transform.position, direction, 5f, collisions);

		if (wallHit.collider != null && wallHit.distance < distanceToCollision) {
			playerMovementController.isWall = true;
			playerMovementController.canMove = false;
			playerMovementController.wallSlide = true;
		} else {
			playerMovementController.canMove = true;
			playerMovementController.wallSlide = false;
		}
	}

	private void handleAnimations ()
	{
		playerAnimationController.walk = playerMovementController.walking;
		playerAnimationController.jump = !playerMovementController.isGround;
		playerAnimationController.wallSlide = playerMovementController.wallSlide;
	}
}

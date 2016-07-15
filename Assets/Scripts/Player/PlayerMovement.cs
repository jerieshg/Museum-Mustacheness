using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{
	public float maxSpeed;
	public float jumpForce;
	public float jumpPushForce = 10f;
	public float wallFriction = 1.1f;
	public bool airControl;
	public LayerMask collisions; 

	private Rigidbody2D rigidBody;
	private bool jump;
	private bool doubleJump;
	private bool wallJump;
	private bool isGround;
	private bool isWall;
	private bool isLeft;


	const float distanceToCollision = 0.9f;

	void Start ()
	{
		rigidBody = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		if (!jump) {
			jump = CrossPlatformInputManager.GetButtonDown ("Jump");
		}	
	}

	void FixedUpdate ()
	{

		checkPlayerSurroundings ();	

		float move = CrossPlatformInputManager.GetAxis ("Horizontal");

		if (isGround || airControl) {
			float fallSpeed = (!isWall) ? rigidBody.velocity.y : rigidBody.velocity.y / wallFriction;
			rigidBody.velocity = new Vector2 (move * maxSpeed, fallSpeed);
		}

		if (jump) {
			bool canJump = false;
			float mJumpForce = jumpForce;

			if (isGround) {
				doubleJump = true;
				canJump = true;
				wallJump = true;
			}else if (doubleJump) {
				doubleJump = false;
				canJump = true;
			}else if (wallJump && isWall) {
				wallJump = false;
				canJump = true;
				mJumpForce = jumpForce * 1.8f;
			}

			if (canJump) {
				rigidBody.AddForce (new Vector2 (jumpPushForce, mJumpForce));
			}
		} 

		if (move > 0 && isLeft) {
			flip ();
		} else if (move < 0 && !isLeft) {
			flip ();
		}

		jump = false;
	}

	private void checkPlayerSurroundings ()
	{
		isGround = false;
		isWall = false;

		RaycastHit2D hit = Physics2D.Raycast (transform.position, -Vector2.up, 2f, collisions);
		if (hit.collider != null && hit.distance< distanceToCollision) {
			isGround = true;
		}

		Vector2 direction = (isLeft) ? Vector2.left : Vector2.right;

		hit = Physics2D.Raycast (transform.position, direction, 5f, collisions);

		if ( hit.collider != null && hit.distance < distanceToCollision){
			isWall = true;
		}
	}

	private void flip(){
		// Switch the way the player is labelled as facing
		isLeft = !isLeft;

		//Multiply the player's x local cale by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}

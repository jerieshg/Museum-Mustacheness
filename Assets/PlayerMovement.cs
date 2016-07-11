using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{
	public float maxSpeed;
	public float jumpForce;
	public bool airControl;
	public LayerMask whatIsGround;
	public LayerMask whatIsWall;
	private Rigidbody2D rigidBody;
	private bool jump;
	private bool doubleJump;
	private Transform groundCheck;
	private Transform wallCheck;
	private bool isGround;
	private bool isWall;

	const float groundedRadius = .2f;

	void Start ()
	{
		rigidBody = GetComponent<Rigidbody2D> ();
		groundCheck = transform.Find ("GroundCheck");
		wallCheck = transform.Find ("WallCheck");
	}

	void OnDrawGizmos ()
	{
		var forward = transform.TransformDirection (Vector3.up) * 10;
		Gizmos.color = Color.green;
		Gizmos.DrawLine (transform.position, forward);
	}

	void Update ()
	{
		
		if (!jump) {
			jump = CrossPlatformInputManager.GetButtonDown ("Jump");
		}	


	}

	void FixedUpdate ()
	{

		checkIsGround ();	

		float move = CrossPlatformInputManager.GetAxis ("Horizontal");

		if (isGround || airControl) {
			

			rigidBody.velocity = new Vector2 (move * maxSpeed, rigidBody.velocity.y);
		}

		if (isGround && jump) {
			rigidBody.AddForce (new Vector2 (0f, jumpForce));
			doubleJump = true;
		} else if (isWall && jump) {
			
		} else if (jump && doubleJump) {
			rigidBody.AddForce (new Vector2 (0f, jumpForce));
			doubleJump = false;
		}

		jump = false;
	}

	private void checkIsGround ()
	{
		isGround = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll (groundCheck.position, groundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			
			if (colliders [i].gameObject != gameObject)
				isGround = true;
		}

	}
}

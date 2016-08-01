using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovementController
{
	//Control Variables
	public float wallFriction = 1.1f;
	public bool airControl = true;
	public bool walking;
	public bool jump;

	//Player Movement Variables
	public bool canJump;
	public bool canDoubleJump;
	public bool wallJump;
	public bool isGround;
	public bool isWall;
	public bool isLeft;
	//UI Buttons Variables
	public bool movingLeft;
	public bool movingRight;
	public bool jumping;
	public float distanceToCollision = 0.9f;

	private PlayerController player;

	public PlayerMovementController (PlayerController player)
	{
		this.player = player;	
	}

	public void move ()
	{
		float move = 0f;
		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		 move = CrossPlatformInputManager.GetAxis ("Horizontal");
		#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
		if (movingLeft && !movingRight) {
			move = Vector3.left.x;
		}
		if (movingRight && !movingLeft) {
			move = Vector3.right.x; 
		}
		#endif
		walking = Mathf.Abs (move) > 0;

		if (isGround || airControl) {
			float fallSpeed = (!isWall) ? player.rigidBody.velocity.y : player.rigidBody.velocity.y / wallFriction;
			player.rigidBody.velocity = new Vector2 (move * player.maxSpeed, fallSpeed);
		}

		jump = false;

		if (canJump) {
			float mJumpForce = player.jumpForce;

			if (isGround) {
				canDoubleJump = true;
				jump = true;
				wallJump = true;
			} else if (canDoubleJump) {
				canDoubleJump = false;
				jump = true;
			} else if (wallJump && isWall) {
				wallJump = false;
				jump = true;
				mJumpForce = player.jumpForce * 1f;
			}

			if (jump) {
				player.rigidBody.velocity = new Vector2 (player.rigidBody.velocity.x, 0);
				player.rigidBody.AddForce (new Vector2 (player.jumpPushForce, mJumpForce));
			}
		} 

		if (move > 0 && isLeft) {
			flip ();
		} else if (move < 0 && !isLeft) {
			flip ();
		}

		canJump = false;
		jumping = false;
	}

	public void checkJump ()
	{
		if (!canJump) {
			#if UNITY_STANDALONE || UNITY_WEBPLAYER
			canJump = CrossPlatformInputManager.GetButtonDown ("Jump");
			#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
			canJump = jumping;
			#endif
		}	
	}



	private void flip ()
	{
		// Switch the way the player is labelled as facing
		isLeft = !isLeft;

		//Multiply the player's x local cale by -1
		Vector3 theScale = player.transform.localScale;
		theScale.x *= -1;
		player.transform.localScale = theScale;
	}
}

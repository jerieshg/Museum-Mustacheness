using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour {

	private Animator playerAnimator;

	[Header("Player Animation States:")]
	public bool walk = false;
	public bool jump = false;
	public bool attack = false;
	public bool throwMarker = false;
	public bool wallSlide = false;
	public bool hit = false;
	public bool dead = false;

	void Start()
	{
		playerAnimator = GetComponent<Animator>();
	}

	void Update()
	{
		animateWalk();
		animateJump();
		animateAttack();
		animateThrowMarker ();
		animateWallSlide ();
		animateHit ();
		animateDead();
	}

	void animateAttack()
	{
		playerAnimator.SetBool("attack",attack);
	}

	void animateJump()
	{
		playerAnimator.SetBool("jump",jump);
	}

	void animateWalk()
	{
		playerAnimator.SetBool("walk",walk);
	}

	void animateThrowMarker(){
		playerAnimator.SetBool ("throw", throwMarker);
	}

	void animateWallSlide()
	{
		playerAnimator.SetBool("wall slide",wallSlide);
	}

	void animateHit()
	{
		playerAnimator.SetBool("hit",hit);
	}

	void animateDead()
	{
		playerAnimator.SetBool("dead",dead);
	}
}

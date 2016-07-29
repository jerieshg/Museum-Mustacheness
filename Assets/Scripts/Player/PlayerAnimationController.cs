﻿using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour {

	private Animator playerAnimator;

	[Header("Player Animation States:")]
	public bool walk = false;
	public bool jump = false;
	public bool attack = false;
	public bool turnSprite = true;
	public bool dead = false;

	void Start()
	{
		playerAnimator = GetComponent<Animator>();
	}

	
	void Update()
	{
		spriteTurnCheck();
		animateWalk();
		animateJump();
		animateAttack();
		animateDead();
	}

	void animateDead()
	{
		playerAnimator.SetBool("dead",dead);
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

	void spriteTurnCheck()
	{
//		if(InputButtonManager.inputButtonManager.onAction && turnSprite)
//		{
//			if(Input.GetKey(InputButtonManager.inputButtonManager.moveLeft))
//			{
//				transform.localScale = new Vector3(-1,1,1);
//			} 
//			else if(Input.GetKey(InputButtonManager.inputButtonManager.moveRight))
//			{
//				transform.localScale = new Vector3(1,1,1);
//			}
//		}
	}
}
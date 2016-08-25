using UnityEngine;
using System.Collections;

public class EnemyAnimationController : MonoBehaviour {

	private Animator enemyAnimator;

	[Header("Enemy Animation States:")]
	public bool patrolWalk = false;
	public bool attack = false;
	public bool hit = false;
	public bool dead = false;

	void Start()
	{
		enemyAnimator = GetComponent<Animator>();
	}

	void Update()
	{
		animatePatrolWalk();
		animateAttack();
		animateHit ();
		animateDead();
	}

	void animateAttack()
	{
		enemyAnimator.SetBool("attack",attack);
	}

	void animatePatrolWalk()
	{
		enemyAnimator.SetBool("patrolWalk", patrolWalk);
	}

	void animateHit()
	{
		enemyAnimator.SetBool("hit",hit);
	}

	void animateDead()
	{
		enemyAnimator.SetBool("dead",dead);
	}
}

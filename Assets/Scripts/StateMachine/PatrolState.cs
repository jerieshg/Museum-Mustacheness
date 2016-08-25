using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyState {

	private readonly StatePatternEnemy enemy;
	private int nextWaypoint;
	private float alertTimer;

	public PatrolState(StatePatternEnemy statePatternEnemy){
		enemy = statePatternEnemy;
	}

	public void UpdateState(){
		Sense ();
		Look ();
		Patrol ();
	}

	public void OnTriggerEnter2D (Collider2D other){

	}

	public void ToPatrolState(){
		Debug.Log ("Can't transition to same state");
	}

	public void ToAlertState(){
		enemy.currentState = enemy.alertState;
		alertTimer = 0;
	}

	public void ToChaseState(){
		enemy.currentState = enemy.chaseState;
	}

	private void Look(){
		Vector3 direction = (enemy.rigidBody.velocity.x > 0) ? Vector3.right : Vector3.left;
		RaycastHit2D hit = Physics2D.Raycast (enemy.eyes.transform.position, direction, enemy.sightRange);

		if(hit.collider != null && hit.collider.CompareTag ("Player")){
			enemy.chaseTarget = hit.transform;
			enemy.animationController.patrolWalk = false;
			ToChaseState ();
		}
	}

	private void Sense(){
		alertTimer += Time.deltaTime;

		if ( alertTimer >= enemy.stateChangeDuration &&
			enemy.hitInfo.collider != null && enemy.hitInfo.collider.CompareTag ("Player")) {
			enemy.animationController.patrolWalk = false;
			ToAlertState ();
		}
	}

	private void Patrol(){
		Vector3 destination = enemy.waypoints [nextWaypoint].position;
		enemy.correctDirection (destination);
		enemy.move (destination, enemy.patrolSpeed);

		enemy.animationController.patrolWalk = true;

		float remainingDistance = Vector3.Distance (destination, enemy.transform.position);

		if (remainingDistance <= enemy.stoppingDistance) {
			nextWaypoint = (nextWaypoint + 1) % enemy.waypoints.Length;
		}
	}
}

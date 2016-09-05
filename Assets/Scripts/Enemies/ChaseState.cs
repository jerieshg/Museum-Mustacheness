using UnityEngine;
using System.Collections;

public class ChaseState : IEnemyState {

	private readonly StatePatternEnemy enemy;

	public ChaseState(StatePatternEnemy statePatternEnemy){
		enemy = statePatternEnemy;
	}


	public void UpdateState(){
		Look ();
		Chase ();
	}

	public void OnTriggerEnter2D (Collider2D other){
	}

	public void ToPatrolState(){
		Debug.Log ("Can't transition to patrol state from chase state");
	}

	public void ToAlertState(){
		enemy.animationController.attack = false;
		enemy.currentState = enemy.alertState;
		Debug.Log ("Going to alert state");
	}

	public void ToChaseState(){
		Debug.Log ("Can't transition to same state");
	}

	private void Chase(){
		enemy.resettingPosition = enemy.exceededDistance;
		if (Vector3.Distance (enemy.transform.position, enemy.chaseTarget.transform.position) > enemy.shootingDistance) {
			enemy.move (enemy.chaseTarget.position, enemy.chaseSpeed);
			enemy.animationController.attack = false;
		} else {
			enemy.animationController.attack = true;
		}

	}

	private void Look(){
		Vector3 enemyToTarget = (enemy.chaseTarget.position + enemy.offset) - enemy.eyes.transform.position;

		if (enemy.eyesHitInfo.collider != null && enemy.eyesHitInfo.collider.CompareTag ("Player")) {
			enemy.chaseTarget = enemy.eyesHitInfo.transform;
		} else {
			ToAlertState ();
		}

		enemy.correctDirection (enemy.chaseTarget.transform.position);
	}
}

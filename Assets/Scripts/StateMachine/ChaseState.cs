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
		enemy.currentState = enemy.alertState;
	}

	public void ToChaseState(){
		Debug.Log ("Can't transition to same state");
	}

	private void Chase(){
		enemy.transform.rotation = new Quaternion ();//temporal
		enemy.meshRenderer.material.color = Color.red;
		enemy.resettingPosition = enemy.exceededDistance;
		if (Vector3.Distance (enemy.transform.position, enemy.chaseTarget.transform.position) > enemy.shootingDistance) {
			enemy.move (enemy.chaseTarget.position, enemy.chaseSpeed);
		} else {
			enemy.Shoot ();
		}

	}

	private void Look(){
		Vector3 enemyToTarget = (enemy.chaseTarget.position + enemy.offset) - enemy.eyes.transform.position;

		RaycastHit2D hit = Physics2D.Raycast (enemy.eyes.transform.position, enemyToTarget, enemy.sightRange);

		if (hit.collider != null && hit.collider.CompareTag ("Player")) {
			enemy.chaseTarget = hit.transform;
		} else {
			ToAlertState ();
		}

		enemy.correctDireciton (enemy.chaseTarget.transform.position);
	}
}

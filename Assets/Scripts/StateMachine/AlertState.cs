using UnityEngine;
using System.Collections;

public class AlertState : IEnemyState {

	private readonly StatePatternEnemy enemy;
	private float searchTimer;
	private float rotateTimer;

	public AlertState(StatePatternEnemy statePatternEnemy){
		enemy = statePatternEnemy;
	}

	public void UpdateState(){
		Look ();
		Search ();
	}

	public void OnTriggerEnter2D (Collider2D other){
		
	}

	public void ToPatrolState(){
		enemy.currentState = enemy.patrolState;
		searchTimer = 0;
	}

	public void ToAlertState(){
		Debug.Log ("Can't transition to same state");
	}

	public void ToChaseState(){
		enemy.currentState = enemy.chaseState;
		searchTimer = 0;
	}

	private void Search(){
		enemy.transform.position = new Vector2 (enemy.transform.position.x,enemy.transform.position.y);//stops the player
		searchTimer += Time.deltaTime;
		rotateTimer += Time.deltaTime;

		if (searchTimer >= enemy.searchingDuration) {
			ToPatrolState ();
		} else if (rotateTimer >= 2f) {
			enemy.turnSprite ();
		}
	}

	private void Look(){
		Vector3 direction = (enemy.rigidBody.velocity.x > 0) ? Vector3.right : Vector3.left;
		RaycastHit2D hit = Physics2D.Raycast (enemy.eyes.transform.position, direction, enemy.sightRange);

		if(hit.collider != null && hit.collider.CompareTag ("Player")){
			enemy.chaseTarget = hit.transform;
			ToChaseState ();
		}
	}
}

﻿using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyState {

	private readonly StatePatternEnemy enemy;
	private int nextWaypoint;

	public PatrolState(StatePatternEnemy statePatternEnemy){
		enemy = statePatternEnemy;
	}

	public void UpdateState(){
		Look ();
		Patrol ();
	}

	public void OnTriggerEnter2D (Collider2D other){

		if (other.gameObject.CompareTag ("Player")) {
			ToAlertState ();
		}
	}

	public void ToPatrolState(){
		Debug.Log ("Can't transition to same state");
	}

	public void ToAlertState(){
		enemy.currentState = enemy.alertState;
	}

	public void ToChaseState(){
		enemy.currentState = enemy.chaseState;
	}

	private void Look(){
		Vector3 direction = (enemy.rigidBody.velocity.x > 0) ? Vector3.right : Vector3.left;
		RaycastHit2D hit = Physics2D.Raycast (enemy.eyes.transform.position, direction, enemy.sightRange);

		if(hit.collider != null && hit.collider.CompareTag ("Player")){
			enemy.chaseTarget = hit.transform;
			ToChaseState ();
		}
	}

	private void Patrol(){
		enemy.meshRenderer.material.color = Color.green;

		Vector2 direction = (enemy.isGoingLeft) ? Vector2.left : Vector2.right;
		enemy.move (direction, enemy.patrolSpeed, enemy.maxPatrolSpeed, false);

	}
}
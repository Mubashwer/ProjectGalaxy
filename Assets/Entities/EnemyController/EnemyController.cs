﻿using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour { 
	
	public GameObject enemyPrefab;
	public LevelManager levelManager;
	public float speed = 20f;
	public float enemyCount = 1; 
	public int enemyKilled = 0;
	
	private GameObject enemy;
	private float enemySpawn = 0;
	
	
	// Use this for initialization
	void Start() {
		StartCoroutine(SpawnEnemies());
	}
	
	void Update(){
	}
	
	
	// Spawn enemies every x seconds
	IEnumerator SpawnEnemies() {
		while(true){
			InitiateEnemy();
			yield return new WaitForSeconds(4-0.8f*Mathf.Sqrt(enemySpawn));
			if(EnemiesDead()){
				yield return new WaitForSeconds(1);
				GameWon();
			}
			
		}
	}
	
	// Initiate Enemy Game Objects (as duplicate of enemyPrefab) 
	void InitiateEnemy(){
		if(enemySpawn < enemyCount){ 
			// [-4.0f, 4.0f] is screen width
			// [6.0f] top of the screen
			Vector3 position = new Vector3(Random.Range(-4.0F, 4.0F), 6.0f, 0); 
			GameObject Enemy = Instantiate(enemyPrefab, position, Quaternion.identity) as GameObject;
	
			Enemy.transform.parent = transform;
			enemySpawn++; 
		}

	}
	
	bool EnemiesDead(){
		if(transform.childCount == 0 && enemySpawn == enemyCount){
			return true;
		}
		return false; 

	}
	
	void GameWon(){
		print ("You won!"); 
		//levelManager.LoadLevel("Win");
		Application.LoadLevel("Win");
	}

	
}
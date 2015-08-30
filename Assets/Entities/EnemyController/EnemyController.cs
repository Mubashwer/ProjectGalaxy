using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour { 
	
	public GameObject enemyPrefab;
	public float speed = 20f;
	public float enemyCount = 10; 
	
	private GameObject enemy;
	private float enemySpawn = 0; 
	
	// Use this for initialization
	void Start() {
		StartCoroutine(SpawnEnemies());
	}
	
	
	// Spawn enemies every x seconds
	IEnumerator SpawnEnemies() {
		while(true){
			InitiateEnemy();
			yield return new WaitForSeconds(3);
		}
	}

	
	// Initiate Enemy Game Objects (as duplicate of enemyPrefab) 
	void InitiateEnemy(){
		
		if(enemySpawn < enemyCount){
		
			// [-4.0f, 4.0f] is screen width
			// [6.0f] top of the screen
			Vector3 position = new Vector3(Random.Range(-4.0F, 4.0F), 6.0f, 0); 
			GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity) as GameObject;
			
			// Rnadomize vertical movementspeed 
			enemy.GetComponent<Rigidbody2D>().drag = Random.Range(8,12);

			MoveEnemies();

			enemy.transform.parent = transform;

			enemySpawn++; 
			
		}else{
			print ("You won!");
		}
	}
	
	void MoveEnemies() {
		foreach( Transform child in transform){
			if(Random.value > 0.5f){
				child.transform.localPosition = new Vector3(5, 0, 0);
			}else{
				child.transform.localPosition = new Vector3(-5,0, 0);
			}
		}
	}
	

	
}
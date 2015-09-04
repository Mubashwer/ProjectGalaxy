using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyController : NetworkBehaviour { 
	
	public GameObject enemyPrefab;
	public LevelManager levelManager;
	public float speed = 20f;
	public float enemyCount = 1; 
	public int enemyKilled = 0;

	private float enemySpawn = 0;
	
	
	// Use this for initialization
	void Start() {
        if (!isServer) return;
        StartCoroutine(SpawnEnemies());
	}
	
	void Update(){
	}


    // Spawn enemies every x seconds
    IEnumerator SpawnEnemies() {
		while(true){
			InitiateEnemy();
			yield return new WaitForSeconds(Random.Range(1.0f,3.5f));
			/*if(EnemiesDead()){
				yield return new WaitForSeconds(1);
				RpcGameWon();
			}*/
			
		}
	}
	
	// Initiate Enemy Game Objects (as duplicate of enemyPrefab) 
	void InitiateEnemy(){
        
        if (enemySpawn < enemyCount){ 
			// [-4.0f, 4.0f] is screen width
			// [6.0f] top of the screen
			Vector3 position = new Vector3(Random.Range(-2.0F, 2.0F), 5.5f, 0);
			GameObject Enemy = Instantiate(enemyPrefab, position, Quaternion.identity) as GameObject;
	            

            NetworkServer.Spawn(Enemy);
			enemySpawn++; 
		}

	}
	
	bool EnemiesDead(){
		if(transform.childCount == 0 && enemySpawn == enemyCount){
			return true;
		}
		return false; 

	}
	
    [ClientRpc]
	void RpcGameWon(){
		print ("You won!"); 
		//levelManager.LoadLevel("Win");
		Application.LoadLevel("Win");
	}

	
}
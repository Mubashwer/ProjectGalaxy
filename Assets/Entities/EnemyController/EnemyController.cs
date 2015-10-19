using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class EnemyController : NetworkBehaviour { 
	
	public GameObject weakEnemyPrefab;
	public GameObject strongEnemyPrefab; 

	public float minWeakEnemyWait;
	public float maxWeakEnemyWait; 
	public float minStrongEnemyWait;
	public float maxStrongEnemyWait;

    public GameManager.Difficulty CurrentAIDifficulty { get; set; }

    // Private reference for this class only
    private static EnemyController _instance;

    private bool _enabled = false;
    public bool Enabled {
        get {
            return _enabled;
        }
        set {
            _enabled = value;
            if (value) {
                StopAllCoroutines();
                StartCoroutine(SpawnWeakEnemies());
                StartCoroutine(SpawnStrongEnemies());
            }
            else {
                StopAllCoroutines();
            }
        }
    }

    //Public reference that other classes will use
    public static EnemyController instance {
        get {
            // Get instance from scene if it hasn't been set
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<EnemyController>();
                if (_instance == null) {
                    _instance = (Instantiate(Resources.Load("EnemyController")) as GameObject).GetComponent<EnemyController>();
                }
            }
            // Reuse in other scenes	
            if(_instance != null) DontDestroyOnLoad(_instance.gameObject);
            return _instance;
        }
    }

    void Awake() {
        if (_instance == null) {
            // Make the first instance the singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            // Destroy this if another exists
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }


    [Server]
    IEnumerator SpawnWeakEnemies() {
        CurrentAIDifficulty = GameManager.instance.CurrentPlayerDifficulty;
        while (true){
			InitiateEnemy("weak");
			yield return new WaitForSeconds(Random.Range(minWeakEnemyWait,maxWeakEnemyWait));
		}
	}
	
	// Spawn strong enemies every x seconds
	[Server]
	IEnumerator SpawnStrongEnemies() {
        CurrentAIDifficulty = GameManager.instance.CurrentPlayerDifficulty;
        while (true){
			InitiateEnemy("strong");
			yield return new WaitForSeconds(Random.Range(minStrongEnemyWait,maxStrongEnemyWait));
		}
	}
	
	
	// Initiate Enemy Game Objects (as duplicate of enemyPrefab) 
	void InitiateEnemy(string enemy){

        if (!NetworkServer.active) return;
        // [-4.0f, 4.0f] is screen width
		// [6.0f] top of the screen
		
        if (enemy == "weak") {
			Vector3 position1 = new Vector3(Random.Range(-2.0F, 2.0F), 5.5f, 0);
			GameObject weakEnemy = Instantiate(weakEnemyPrefab, position1, Quaternion.identity) as GameObject;
        	NetworkServer.Spawn(weakEnemy);
        	
        }
        
		else if ( enemy == "strong") {
			Vector3 position2 = new Vector3(Random.Range(-2.0F, 2.0F), 5.5f, 0);
			GameObject strongEnemy = Instantiate(strongEnemyPrefab, position2, Quaternion.identity) as GameObject;
			NetworkServer.Spawn(strongEnemy);
			
		}
        
	}

}
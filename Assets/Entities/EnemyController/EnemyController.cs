using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

<<<<<<< HEAD
public class EnemyController : NetworkBehaviour {

    public GameObject enemyPrefab;
=======
public class EnemyController : NetworkBehaviour { 
	
	public GameObject weakEnemyPrefab;
	public GameObject strongEnemyPrefab; 
	public GameObject strongEnemyPrefab222; 
	public int minWeakEnemy;
	public int maxWeakEnemy; 
	public int minStrongEnemy;
	public int maxStrongEnemy; 
	public BossAI bossAI;
>>>>>>> 9bdb17ca117a615d2ed0f48d23a79fec976c7ba6

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
                StartCoroutine(SpawnEnemies());
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
                // Reuse in other scenes	
                DontDestroyOnLoad(_instance.gameObject);
            }
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

    // Use this for initialization
    void Start() {
        if (!isServer) return;
    }

    void Update() {
        if (!isServer) return;
<<<<<<< HEAD
    }

    public override void OnStartServer() {
        //StartCoroutine(SpawnEnemies());
        //Debug.Log("serverstarted");
=======
		if (CoroutinesStopped){
			StartCoroutine(SpawnWeakEnemies()); 
			StartCoroutine(SpawnStrongEnemies());
		}
    }

    public override void OnStartServer() {
        StartCoroutine(SpawnWeakEnemies());
		StartCoroutine(SpawnStrongEnemies());
        Debug.Log("serverstarted");
>>>>>>> 9bdb17ca117a615d2ed0f48d23a79fec976c7ba6
    }


    // Spawn weak enemies every x seconds
    [Server]
<<<<<<< HEAD
    IEnumerator SpawnEnemies() {
        while (true) {
            InitiateEnemy();
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
            /*if(EnemiesDead()){
                yield return new WaitForSeconds(1);
                RpcGameWon();
            }*/

        }
    }

    // Initiate Enemy Game Objects (as duplicate of enemyPrefab) 
    void InitiateEnemy() {


        // [-4.0f, 4.0f] is screen width
        // [6.0f] top of the screen
        Vector3 position = new Vector3(Random.Range(-2.0F, 2.0F), 5.5f, 0);
        GameObject Enemy = Instantiate(enemyPrefab, position, Quaternion.identity) as GameObject;


        if (NetworkServer.active) NetworkServer.Spawn(Enemy);
    }
=======
    IEnumerator SpawnWeakEnemies() {
        CoroutinesStopped = false;
        while (true){
			InitiateEnemy("weak");
			yield return new WaitForSeconds(Random.Range(minWeakEnemy,maxWeakEnemy));
			/*if(EnemiesDead()){
				yield return new WaitForSeconds(1);
				RpcGameWon();
			}*/
			
		}
	}
	
	// Spawn strong enemies every x seconds
	[Server]
	IEnumerator SpawnStrongEnemies() {
		CoroutinesStopped = false;
		while (true){
			InitiateEnemy("strong");
			yield return new WaitForSeconds(Random.Range(minStrongEnemy,maxStrongEnemy));
			/*if(EnemiesDead()){
				yield return new WaitForSeconds(1);
				RpcGameWon();
			}*/
			
		}
	}
	
	
	// Initiate Enemy Game Objects (as duplicate of enemyPrefab) 
	void InitiateEnemy(string enemy){

		// [-4.0f, 4.0f] is screen width
		// [6.0f] top of the screen
		

        if (NetworkServer.active && enemy == "weak") {
        	Debug.Log ("Weak enemy spawned");
			Vector3 position1 = new Vector3(Random.Range(-2.0F, 2.0F), 5.5f, 0);
			GameObject weakEnemy = Instantiate(weakEnemyPrefab, position1, Quaternion.identity) as GameObject;
        	NetworkServer.Spawn(weakEnemy);
        	
        }
        
		if (NetworkServer.active && enemy == "strong") {
			Debug.Log("Strong enemy spawned");
			Vector3 position2 = new Vector3(Random.Range(-2.0F, 2.0F), 5.5f, 0);
			GameObject strongEnemy = Instantiate(strongEnemyPrefab, position2, Quaternion.identity) as GameObject;
			NetworkServer.Spawn(strongEnemy);
			
		}
        
	}
	
	void InitiateBoss(){
		//bossAI.isAlive = true; 
		//GameObject boss = Instantiate(boss, transform.position, 0) as GameObject; 
		//NetworkServer.Spawn(boss); 
	
	}
>>>>>>> 9bdb17ca117a615d2ed0f48d23a79fec976c7ba6

}
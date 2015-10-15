using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyController : NetworkBehaviour {

    public GameObject enemyPrefab;

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
    }

    public override void OnStartServer() {
        //StartCoroutine(SpawnEnemies());
        //Debug.Log("serverstarted");
    }


    // Spawn enemies every x seconds
    [Server]
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

}
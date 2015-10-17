using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// Spawns powerUps from time to time
public class PowerUpController : NetworkBehaviour {

    // Use this for initialization
    public string[] powerUps; // list of names of powerUps
    private int id; //unique id of every pair of item and corresponding powerUp dropped

    private bool _enabled = false;
    public bool Enabled {
        get {
            return _enabled;
        }
        set {
            _enabled = value;
            if (value) {
                StopAllCoroutines();
                StartCoroutine(SpawnPowerUps());
            }
            else {
                StopAllCoroutines();
            }
        }
    }

    // Private reference for this class only
    private static PowerUpController _instance;

    //Public reference that other classes will use
    public static PowerUpController instance {
        get {
            // Get instance from scene if it hasn't been set
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<PowerUpController>();
                if (_instance == null) {
                    _instance = (Instantiate(Resources.Load("PowerUpController")) as GameObject).GetComponent<PowerUpController>();
                }
            }
            // Reuse in other scenes	
            DontDestroyOnLoad(_instance.gameObject);
            return _instance;
        }
    }

    void Awake() {
        id = 0;
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


    void Start () {

        if (!isServer) return;
    }

    
    // Spawn powerUps every x seconds
    IEnumerator SpawnPowerUps() {
        while (true) {
            InitiatePowerUp();
            yield return new WaitForSeconds(Random.Range(2.0f, 3.0f));
        }
    }

    // Initiate powerups 
    void InitiatePowerUp() {
        if (!isServer) return;
        int index = Random.Range(0, powerUps.GetLength(0)); // get random powerup index
        Vector3 position = new Vector3(Random.Range(-2, 2), 5.5f, 0); // get random spawn position
        //RpcDropPowerUp(index, position, id++);
        GameObject item = Instantiate(Resources.Load(powerUps[index] + "item"), position, Quaternion.identity) as GameObject;
        item.GetComponent<PowerUpItem>().SetId(id++);
        NetworkServer.Spawn(item);
    }

    /*[ClientRpc]
    void RpcDropPowerUp(int index, Vector3 position, int itemID) {
        GameObject item = Instantiate(Resources.Load(powerUps[index] + "item"), position, Quaternion.identity) as GameObject;
        item.GetComponent<PowerUpItem>().SetId(itemID);
    }*/

    // Update is called once per frame
    public override void OnStartServer () {
        
    }

    void Update() {
        if (!isServer) return;
    }


}

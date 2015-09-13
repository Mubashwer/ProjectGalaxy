using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// Spawns powerUps from time to time
public class PowerUpController : NetworkBehaviour {

    // Use this for initialization
    public string[] powerUps = { "Shield"};
    private GameObject powerUp;

    void Start () {
        if (!isServer) return;
        StartCoroutine(SpawnPowerUps());
    }

    
    // Spawn powerUps every x seconds
    IEnumerator SpawnPowerUps() {
        while (true) {
            InitiatePowerUp();
            yield return new WaitForSeconds(Random.Range(2.0f, 3.0f));
        }
    }

    // Initiate powerups 
    [Server]
    void InitiatePowerUp() {
        
        int index = Random.Range(0, powerUps.GetLength(0)); // get random powerup index
        Vector3 position = new Vector3(Random.Range(-2, 2), 5.5f, 0); // get random spawn position
        powerUp = Instantiate(Resources.Load(powerUps[index] + "item"), position, Quaternion.identity) as GameObject;
        if (NetworkServer.active) NetworkServer.Spawn(powerUp);

    }

    // Update is called once per frame
    void Update () {
	
	}
}

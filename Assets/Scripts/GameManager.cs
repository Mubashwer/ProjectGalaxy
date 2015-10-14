using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    // Private reference for this class only
    private static GameManager _instance;

    public GameMode CurrentGameMode { get; set; }

    public enum GameMode {
        None,
        SinglePlayer,
        MultiPlayerClient,
        MultiPlayerHost
    }

    //Public reference that other classes will use
    public static GameManager instance
    {
        get {
            // Get instance from scene if it hasn't been set
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<GameManager>();
                // Reuse in other scenes	
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Awake()
    {
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


    // Find local player
    public PlayerController FindLocalPlayer() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        PlayerController player = null;
        if (players.GetLength(0) > 0) {
            foreach (GameObject p in players) {
                if (p.GetComponent<PlayerController>().isLocalPlayer) {
                    player = p.GetComponent<PlayerController>();
                    break;
                }
            }
        }
        return player;
    }
    
    // Check if all the players are dead
	public bool AllPlayersDead() {
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		if (players.GetLength(0) > 0) {
			foreach (GameObject p in players) {
				if (p.GetComponent<PlayerController>().lives > 0) {
					return false;
				} 
			}
		}
		return true;
	}

    public PlayerController GetPlayer(short id) {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        PlayerController player = null;
        if (players.GetLength(0) > 0) {
            foreach (GameObject p in players) {
                if (p.GetComponent<PlayerController>().playerControllerId == id) {
                    player = p.GetComponent<PlayerController>();
                    break;
                }
            }
        }
        return player;
    }

    void Update() {
        if(!EnemyController.instance) Instantiate(Resources.Load("EnemyController"));
    }

}

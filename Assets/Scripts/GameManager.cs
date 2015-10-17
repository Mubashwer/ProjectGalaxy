using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    // Private reference for this class only
    private static GameManager _instance;
    public enum GameMode {
        None,
        SinglePlayer,
        MultiPlayerClient,
        MultiPlayerHost
    }
    public GameMode CurrentGameMode { get; set; }

    public enum Difficulty {
        Easy,
        Normal,
        Hard,
    }
    private Difficulty _difficulty;
    public Difficulty CurrentPlayerDifficulty { get; set; }


    //Public reference that other classes will use
    public static GameManager instance
    {
        get {
            // Get instance from scene if it hasn't been set
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<GameManager>();
                if(_instance == null) {
                    _instance = (Instantiate(Resources.Load("GameManager")) as GameObject).GetComponent<GameManager>();
                } 
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
        CurrentPlayerDifficulty = Difficulty.Normal;
        CurrentGameMode = GameMode.None;
    }

	// Host has disconnected the game
	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if(info == NetworkDisconnection.LostConnection){
			Debug.Log("Lost connection to the server");
		}
	}

    
    // Check if all the players are dead

    void Update() {
    }

}

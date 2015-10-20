using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Advertisements;

public class LevelManager : MonoBehaviour {


    // Private reference for this class only
    private static LevelManager _instance;
    //Public reference that other classes will use
    public static LevelManager instance {
        get {
            // Get instance from scene if it hasn't been set
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<LevelManager>();
                // Reuse in other scenes
                if (_instance == null) {
                    _instance = (Instantiate(Resources.Load("LevelManager")) as GameObject).GetComponent<LevelManager>();
                }
            }

            DontDestroyOnLoad(_instance.gameObject);
            return _instance;
        }
    }
    private bool paused = false;

    // Data for GameOver canvas
    public int Score { get; set; }
    public string Message { get; set; }

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
        Message = "host disconnected";
        Score = 0;
    }
    
    void Start() {
    }

    public void GameOver(string message, int score) {
        Message = message;
        Score = score;
        LoginManager.instance.PostScoreToLeaderBoard(score);
        Instantiate(Resources.Load("CanvasGameOver"));
        
    }

    public void PauseGame(){
		if(paused == false){
			Time.timeScale = 0;
			paused = true;
		}
		else if(paused == true){
			Time.timeScale = 1.0f;
			paused = false;
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

    public void LoadLevel(string levelName, bool showAd) {
        if (!showAd) Application.LoadLevel(levelName);
        else StartCoroutine(ShowAdThenLoad(levelName));
    }

    IEnumerator ShowAdThenLoad(string levelName) {
        while (!Advertisement.IsReady())
            yield return null;
        try {
                Advertisement.Show();
        }
        catch { }
        finally {
            Application.LoadLevel(levelName);
        }

    } 


}
	
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;


public class MenuControl : MonoBehaviour {

    private GameObject mainMenuCanvas;
    private GameObject multiplayerMenuCanvas;
    private GameObject optionsCanvas;


    public void Start() {
        mainMenuCanvas = GameObject.Find("CanvasMain");
        multiplayerMenuCanvas = GameObject.Find("CanvasMultiplayer");
        optionsCanvas = GameObject.Find("CanvasOptions");
        optionsCanvas.SetActive(false);
    }

    public void OnClickedSinglePlayer() {
        GameManager.instance.CurrentGameMode = GameManager.GameMode.SinglePlayer;
        Application.LoadLevel("Level_01");
        NetworkManager networkManager = NetworkManagerCustom.instance;
        networkManager.StartHost();
        EnemyController.instance.Enabled = true;
        PowerUpController.instance.Enabled = true;
    }

    public void OnClickedLocalMultiPlayer() {
        mainMenuCanvas.SetActive(false);
        multiplayerMenuCanvas.SetActive(true);
    }

    public void OnClickedInGameQuit() {
        
        GameManager.GameMode mode = GameManager.instance.CurrentGameMode;
        if (GameManager.instance.Paused) {
            var pauseButton = GameObject.Find("PauseButton");
            if (pauseButton) {
                pauseButton.GetComponent<PauseButton>().PauseGame(); // Unpause if game is quit while being paused
            }
        }
        // Stop Game and network connection 
        GameManager.instance.CurrentGameMode = GameManager.GameMode.None;
        NetworkManagerCustom networkManager = NetworkManagerCustom.instance;
        networkManager.StopGame();
        if ((mode == GameManager.GameMode.MultiPlayerHost || mode == GameManager.GameMode.SinglePlayer) && (NetworkServer.active || NetworkClient.active)) {
            networkManager.StopHost();
        }
        else if (mode == GameManager.GameMode.MultiPlayerClient && NetworkClient.active) {
            networkManager.StopClient();
        }
        Application.LoadLevel("Menu");
    }

    public void OnClickedMultiplayerMenuBack() {
        mainMenuCanvas.SetActive(true);
        multiplayerMenuCanvas.SetActive(false);
    }

    public void OnClickedMultiplayerMenuClient() {
        GameManager.instance.CurrentGameMode = GameManager.GameMode.MultiPlayerClient;
        Application.LoadLevel("Level_01");
    }

    public void OnClickedMultiplayerMenuServer() {
        GameManager.instance.CurrentGameMode = GameManager.GameMode.MultiPlayerHost;
        Application.LoadLevel("Level_01");
        NetworkManager networkManager = NetworkManagerCustom.instance;
        networkManager.StartHost();
    }

    public void OnClickedLeaderBoard() {
        // show leaderboard UI
        try {
            LoginManager.instance.ShowLeaderBoard();
        }
        catch { }

    }


    /* OPTIONS */

    public void OnClickedOptions() {
        mainMenuCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }

    public void OnDifficultyChanged() {
        Slider slider = GameObject.Find("DifficultySlider").GetComponent<Slider>();
        GameManager.instance.CurrentPlayerDifficulty = (GameManager.Difficulty)Mathf.RoundToInt(slider.value);
    }

    public void OnMusicToggleClicked() {
        Toggle toggle = GameObject.Find("MusicToggle").GetComponent<Toggle>();
        LevelManager.instance.GetComponent<AudioSource>().mute = !toggle.isOn;
    }

    public void OnClickedOptionsMenuBack() {
        mainMenuCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
    }

}

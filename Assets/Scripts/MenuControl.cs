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
        
        GameManager.instance.CurrentGameMode = GameManager.GameMode.None;
        try {
            NetworkManagerCustom networkManager = NetworkManagerCustom.instance;
            if (NetworkServer.active) {
                networkManager.StopServer();
            }
            if (NetworkClient.active) {
                networkManager.StopClient();
            }
            networkManager.StopGame();
        }
        catch {
        }
        finally {
            Application.LoadLevel("Menu");
        }
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
        //GameManager.instance.CurrentGameMode = GameManager.GameMode.MultiPlayerClient;
        //Application.LoadLevel("Level_01");
        //NetworkManager networkManager = NetworkManagerCustom.instance;
        //networkManager.StartClient();
    }

    public void OnClickedOptions() {
        mainMenuCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }
    public void OnClickedOptionsMenuBack() {
        mainMenuCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
    }
    public void OnDifficultyChanged() {
        Slider slider = GameObject.Find("DifficultySlider").GetComponent<Slider>();
        GameManager.instance.CurrentPlayerDifficulty = (GameManager.Difficulty)Mathf.RoundToInt(slider.value);
    }

}

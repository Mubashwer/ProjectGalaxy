using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {
    public void LoadScene(int index) {
        Application.LoadLevel(index);
    }

    public void StartSinglePlayerGame() {
        // setup parameters to enable single player only
        GameManager.instance.SinglePlayer = true;
        Application.LoadLevel("Level_01");
    }

    public void StartMultiplayerGame() {
        // setup parameters to enable multiplayer menu
        GameManager.instance.SinglePlayer = false;
        Application.LoadLevel("Level_01");
    }

    public void OpenLeaderboard() {
        Application.LoadLevel("Leaderboard");
    }

    public void OpenMainMenu() {
        Application.LoadLevel("MainMenu");
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{

    private PlayerController player;
    private Text myText;
    private GameManager gameManager;
    bool playerSet = false;

    void Start() {
        myText = GetComponent<Text>();
        myText.text = 0.ToString();
        gameManager = GameManager.instance;
    }

    void FindPlayer() {
        if (!playerSet) {
            player = gameManager.FindLocalPlayer();
            if (player) playerSet = true;
        }
    }

    void Update() {
        FindPlayer();
        if (player) {
            myText.text = player.getScore().ToString();
        }
    }

    public void Reset() {
        myText.text = 0.ToString();
    }
}

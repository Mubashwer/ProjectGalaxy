using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{

    private PlayerController player;
    private Text myText;
    bool playerSet = false;

    void Start() {
        myText = GetComponent<Text>();
        myText.text = 0.ToString();
    }

    void FindPlayer() {
        if (!playerSet) {
            player = LevelManager.instance.FindLocalPlayer();
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

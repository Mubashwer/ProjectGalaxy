using UnityEngine;
using System.Collections;

public class HUDLives : MonoBehaviour {

    // Use this for initialization

    public Sprite twoLivesSprite;
    public Sprite zeroLivesSprite;
    public Sprite oneLifeSprite { get; set; }

    private int currentLives;
    private bool playerSet = false;
    private PlayerController player;
    void Start () {
        oneLifeSprite = GetComponent<SpriteRenderer>().sprite; // original sprite is HUD for one life
	}

    void FindPlayer() {
        if (!playerSet) {
            player = LevelManager.instance.FindLocalPlayer();
            if (player) {
                playerSet = true;
                currentLives = player.GetCurrentRemainingLives();
            }
        }
    }
    // Update is called once per frame
    void Update () {
        FindPlayer();
        int newCurrentLives = player.GetCurrentRemainingLives();
        if (currentLives == newCurrentLives) return;
        currentLives = newCurrentLives;

        // If there is a change in lives then select appropriate HUD sprite
        if (currentLives <= 0) GetComponent<SpriteRenderer>().sprite = zeroLivesSprite;
        else if (currentLives == 1) GetComponent<SpriteRenderer>().sprite = oneLifeSprite;
        else if (currentLives == 2) GetComponent<SpriteRenderer>().sprite = twoLivesSprite;

    }
}

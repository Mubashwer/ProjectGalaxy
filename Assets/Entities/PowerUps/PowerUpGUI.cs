using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PowerUpGUI : MonoBehaviour {

    public PlayerController player;
    private bool playerSet;
    private GameManager gameManager;
    private PowerUpItem itemHUD;

    // Use this for initialization
    void Start () {
        gameManager = GameManager.instance;
        playerSet = false;
    }

    bool FindPlayer() {
        if (!playerSet) {
            player = gameManager.FindLocalPlayer();
            if (player) {
                playerSet = true;
            }
        }
        return playerSet;
    }

    // Update is called once per frame
    void Update () {
        if (!FindPlayer() || player.isAlive == false) return; // find local player

        // if powerUp has been used: destory itemHUD
        if (!player.powerUp && itemHUD) {
            itemHUD.Destroy();
            gameObject.GetComponent<Image>().fillAmount = 0;
        }
        if (!itemHUD && player.item) {
            GotPowerUp();
        }

        float ratio;
        if(player.powerUp) {
            if (player.powerUp.hasTimer) {
                ratio = player.powerUp.GetTimer() / player.powerUp.duration;
            }
            else {
                ratio = player.powerUp.GetCounter() / player.powerUp.count;
            }
            GetComponent<Image>().fillAmount = ratio;
        }

	}

    void GotPowerUp() {
        // Get HUD item
        itemHUD = player.item;
        
        // move player.item to HUD
        itemHUD.transform.position = new Vector3(-2, 4, 0);
        gameObject.GetComponent<Image>().fillAmount = 1;
    }

  
}

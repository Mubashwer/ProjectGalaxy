using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PowerUpGUI : NetworkBehaviour {

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
            gameObject.GetComponent<Image>().fillAmount = 0;
            itemHUD.Destroy();
        }
        // if player collected new powerUpItem: remove the older item from HUD and get the new one
        if ((player.item && itemHUD && player.item != itemHUD)) {
            itemHUD.Destroy();
            GotPowerUp();

        } // if player gets an item
        else if (!itemHUD && player.item) {
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
        // Duplicate player.item and delete original (i.e. make player.item local)
        itemHUD = Instantiate(player.item.gameObject).GetComponent<PowerUpItem>();
        player.CmdDestroyPowerUpItem();
        if (player.item) Destroy(player.item.gameObject);
        
        // move player.item to HUD
        itemHUD.transform.position = new Vector3(-2, 4, 0);

        gameObject.GetComponent<Image>().fillAmount = 1;
    }

  
}

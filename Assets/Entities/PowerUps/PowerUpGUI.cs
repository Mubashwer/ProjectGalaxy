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

        
        // if player replace powerUp: replace HUD
        if(itemHUD && player.powerUp && player.powerUp.GetId() != itemHUD.GetId()) {
            gameObject.GetComponent<Image>().fillAmount = 0;
            itemHUD.Destroy();
            GotPowerUp();
        }
        // if powerUp has been used: destory HUD
        if (!player.powerUp && itemHUD) {
            gameObject.GetComponent<Image>().fillAmount = 0;
            itemHUD.Destroy();
        } // if player gets a new powerUp: get new HUD
        if (!itemHUD && player.powerUp) {
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
        itemHUD = (Instantiate(Resources.Load(player.powerUp.powerUpName + "Item")) as GameObject).GetComponent<PowerUpItem>();
        itemHUD.GetComponent<Rigidbody2D>().isKinematic = true; // will stop moving
        itemHUD.GetComponent<Collider2D>().enabled = false; // will no longer collide

        // move player.item to HUD
        itemHUD.transform.position = new Vector3(-2, 4, 0);
        gameObject.GetComponent<Image>().fillAmount = 1;
    }

  
}

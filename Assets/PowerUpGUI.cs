using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PowerUpGUI : NetworkBehaviour {

    private bool powerUpGained;
    private bool hasTimer;
    private float timer;

    public PlayerController player;
    private bool playerSet;
    private GameManager gameManager;

    private PowerUpItem itemHUD;

    // Use this for initialization
    void Start () {
        gameManager = GameManager.instance;
        playerSet = false;
        hasTimer = false;
        powerUpGained = false;
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

        if ((player.item && itemHUD && player.item != itemHUD)) {
            itemHUD.Destroy();
            GetPowerUp();
        }
        else if (!itemHUD && player.item) {
            GetPowerUp();
        }


            if (!player.powerUp)
            powerUpGained = false;

        if (player.powerUp && (!powerUpGained || player.item.replaced)) { // if player just got an player.item, load it into HUD
            player.item.replaced = false;
            //GetPowerUp();
        }
       

        // destroy when timer counts down to zero for some powerups
        if (powerUpGained && hasTimer && player.powerUp.isActivated()) {
            timer -= Time.deltaTime;
            gameObject.GetComponent<Image>().fillAmount = Mathf.Abs(timer) / player.powerUp.duration;
            if (timer <= 0) {
                player.powerUp.SetDectivated(true);
                player.CmdDestroyPowerUp();
                if (player.powerUp.gameObject) Destroy(player.powerUp.gameObject);
                itemHUD.Destroy(); 
                powerUpGained = false;
            }
        }
	}

    void GetPowerUp() {
        // Duplicate player.item and delete original (i.e. make player.item local)
        itemHUD = Instantiate(player.item.gameObject).GetComponent<PowerUpItem>();
        player.CmdDestroyPowerUpItem();
        if (player.item.gameObject) Destroy(player.item.gameObject);
        
        // move player.item to HUD
        itemHUD.transform.position = new Vector3(-2, 4, 0);
        gameObject.GetComponent<Image>().fillAmount = 1;

        powerUpGained = true;
        hasTimer = player.powerUp.hasTimer;
        if (hasTimer)
            timer = player.powerUp.duration;

    }

  
}

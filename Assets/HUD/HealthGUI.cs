using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class HealthGUI : MonoBehaviour
{

    // Use this for initialization
    private PlayerController player; // health
    private float maxHealth; //maximum health of player
    bool playerSet = false;


    void Start() {

    }

    void FindPlayer() {
        if (!playerSet) {
            player = LevelManager.instance.FindLocalPlayer();
            if (player) {
                playerSet = true;
                maxHealth = player.GetMaxHealth();
            }
        }
    }

    // Update is called once per frame
    void Update() {
        float health;
        FindPlayer();
        if (player) {
            health = player.getHealth();
        }
        else {
            health = 0;
        }
        gameObject.GetComponent<Image>().fillAmount = health / maxHealth;
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthGUI : MonoBehaviour {

	// Use this for initialization
	private GameObject player; // health
	private float maxHealth; //maximum health of player
	
	void Start () {
		player = GameObject.Find ("Player");
		if(player)
			maxHealth = player.GetComponent<PlayerController>().maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
		float health;
		if (player){
			health = player.GetComponent<PlayerController>().getHealth();
		}
		else {
			health = 0;
		}
		gameObject.GetComponent<Image>().fillAmount = health/maxHealth;
	}
}

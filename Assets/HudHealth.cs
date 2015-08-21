using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudHealth : MonoBehaviour {

	public int healthy;
	private Text myText;
	

	// Use this for initialization
	void Start(){
		myText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		GameObject player = GameObject.Find ("Player");
		healthy = (int)(player.GetComponent<PlayerController>().health);
		myText.text = healthy.ToString();
		Debug.Log (healthy);
	}
}


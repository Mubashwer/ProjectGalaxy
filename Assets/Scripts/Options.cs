using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Options : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponentInChildren<Slider>().value = (int)GameManager.instance.CurrentPlayerDifficulty;
        GetComponentInChildren<Toggle>().isOn = !LevelManager.instance.GetComponent<AudioSource>().mute;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

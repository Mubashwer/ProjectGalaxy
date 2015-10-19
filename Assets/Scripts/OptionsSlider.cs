using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsSlider : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Slider>().value = (int)GameManager.instance.CurrentPlayerDifficulty;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

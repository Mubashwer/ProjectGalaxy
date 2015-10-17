using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.FindChild("Message").gameObject.GetComponent<Text>().text = LevelManager.instance.Message;
        transform.FindChild("Score").gameObject.GetComponent<Text>().text = "score: " + LevelManager.instance.Score;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

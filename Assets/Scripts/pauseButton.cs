using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseButton : MonoBehaviour {

    public bool paused = false;
    public Sprite pausedSprite;
    private Sprite unPausedSprite;

    void Start() {
        // disable pause in multiplayer
        if (GameManager.instance.CurrentGameMode != GameManager.GameMode.SinglePlayer) {
            gameObject.SetActive(false);
        }
        unPausedSprite = GetComponent<Image>().sprite;
    }

    void Update() {
    }


    public void PauseGame(){
		if(paused == false){
			Time.timeScale = 0;
			paused = true;
            GameManager.instance.Paused = true;
            GetComponent<Image>().overrideSprite = pausedSprite;
        }
		else if(paused == true){
			Time.timeScale = 1.0f;
			paused = false;
            GameManager.instance.Paused = false;
            GetComponent<Image>().overrideSprite = unPausedSprite;
        }
	}
}

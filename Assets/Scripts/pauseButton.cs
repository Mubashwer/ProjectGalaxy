using UnityEngine;
using System.Collections;

public class pauseButton : MonoBehaviour {
	public bool paused = false;
	public void PauseGame(){
		if(paused == false){
			Time.timeScale = 0;
			paused = true;
		}
		else if(paused == true){
			Time.timeScale = 1.0f;
			paused = false;
		}
	}
}

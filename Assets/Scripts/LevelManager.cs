using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public bool paused = false;
		
	public void LoadLevel(string name){
		Debug.Log ("New Level load: " + name);
		Application.LoadLevel (name);
	}
		
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
	
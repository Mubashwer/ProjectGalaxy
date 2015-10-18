using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GooglePlayGames;

public class LoginText : MonoBehaviour {

    private Text myText;

    // Use this for initialization
    void Start() {
        myText = GetComponent<Text>();
        LoginManager.instance.loginButtonText = myText;
        // Check login status every ten seconds
        SetLoginButtonText();
    }

    // Get text for button (sign in if loggin in and signed out otherwise)
    private void SetLoginButtonText() {
        if (LoginManager.instance.Status || PlayGamesPlatform.Instance.IsAuthenticated()) {
            myText.text = "SIGN OUT";
        }
        else {
            myText.text = "SIGN IN";
        }
    }


}

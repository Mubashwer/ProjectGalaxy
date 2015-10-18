using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;


public class LoginManager : MonoBehaviour {

    // Private reference for this class only
    private static LoginManager _instance;
    //Public reference that other classes will use
    public static LoginManager instance {
        get {
            // Get instance from scene if it hasn't been set
            if (_instance == null) {
                _instance = FindObjectOfType<LoginManager>();
                if (_instance == null) {
                    _instance = (Instantiate(Resources.Load("Login")) as GameObject).GetComponent<LoginManager>();
                }
                // Reuse in other scenes	
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Awake() {
        if (_instance == null) {
            // Make the first instance the singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            // Destroy this if another exists
            if (this != _instance)
                Destroy(this.gameObject);
        }
        PlayGamesPlatform.Activate();

    }
    public bool Status {
        get {
            return Social.localUser.authenticated;
        }
    }

    public Text loginButtonText { get; set; }

    // Use this for initialization
    void Start() {
        loginButtonText = GameObject.Find("LoginText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void OnClickedLogin() {
        // Sign out if login button is pressed after signing in
        if (Status) {
            PlayGamesPlatform.Instance.SignOut();
            SetLoginButtonText(false);
        }
        else { // authenticate otherwise
            Social.localUser.Authenticate((bool success) => {
                SetLoginButtonText(success);
            });
        }
        
    }

    // Upload score to Google Play Leaderboard
    public void PostScoreToLeaderBoard(int score) {
        if (!Status) return;
        Social.ReportScore(score, "CgkI6fq2k60YEAIQAA", (bool success) => {
            // handle success or failure
        });
    }

    // Show Leaderboard
    public void ShowLeaderBoard() {
        if (Status) { //show leader board if loggied
            PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkI6fq2k60YEAIQAA");
            return;
        }
        
        // authenticate otherwise then show if successful
        Social.localUser.Authenticate((bool success) => {
            SetLoginButtonText(success);
            if(success) {
                PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkI6fq2k60YEAIQAA");
            }
        });
        

    }

    private void SetLoginButtonText(bool success) {
        if (!loginButtonText) {
            loginButtonText = GameObject.Find("LoginText").GetComponent<Text>();
        }
        if (success) {
            loginButtonText.text = "SIGN OUT";
        }
        else {
            loginButtonText.text = "SIGN IN";
        }
    }
}

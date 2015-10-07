using UnityEngine;
using System.Collections;
using SimpleJSON;

public class PopulateLeaderboard : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("PopulateLeaderboard.Start()");
        //var form = new WWWForm().AddField("UserName", "");
        Debug.Log("Requesting high scores from leaderboard server");
        WWW www = new WWW("http://localhost:8080/Api/GetScores");
        
        string rawText = www.text
            ?? "[]";

        Debug.Log("Parsing high scores JSON data");
        JSONNode node = JSON.Parse(rawText);
        foreach (var scoreRecord in node.AsArray.Childs) {
            int Id = scoreRecord["Id"].AsInt;
            string userUniqueKey = scoreRecord["UserUniqueKey"].Value;
            string userName = scoreRecord["UserName"].Value;
            int score = scoreRecord["Score"].AsInt;
            string TimeString = scoreRecord["Time"].Value;

            Instantiate(new 

            // TODO: Populate the UI with the downloaded leaderboard information

        }

    }
	
	// Update is called once per frame
	void Update () {
    }
}

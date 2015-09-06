using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    // Private reference for this class only
    private static GameManager _instance;

    //Public reference that other classes will use
    public static GameManager instance
    {
        get
        {
            // Get instance from scene if it hasn't been set
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
                // Reuse in other scenes	
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            // Make the first instance the singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            // Destroy this if another exists
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }


    // Find local player
    public GameObject FindLocalPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject player = null;
        if (players.GetLength(0) > 0)
        {
            foreach (GameObject p in players)
            {
                if (p.GetComponent<PlayerController>().isLocalPlayer)
                {
                    player = p;
                    break;
                }
            }
        }
        return player;
    }

}

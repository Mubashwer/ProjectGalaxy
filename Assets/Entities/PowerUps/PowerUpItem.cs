using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PowerUpItem : NetworkBehaviour {

    public string powerUpName;
    private int id; //unique id of item which is also used by corresponding powerUp

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public int GetId() {
        return id;
    }

    public void SetId(int id) {
        this.id = id;
    }

    public void Destroy() {
        Destroy(gameObject);
    }


}

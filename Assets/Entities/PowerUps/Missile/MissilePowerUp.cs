using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MissilePowerUp : PowerUp {

    public GameObject missilePrefab;
    public Missile missile;

    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    // Double Tap: Shoots a homing missile

    public override void DoubleTapEvent() {
        Vector3 bulletPos = player.transform.position;
        bulletPos.y += 0.5f;
        GameObject missileObject = Instantiate(missilePrefab, bulletPos, Quaternion.AngleAxis(90,Vector3.forward)) as GameObject;
        //if (NetworkServer.active) NetworkServer.Spawn(missileObject);
        missile = missileObject.GetComponent<Missile>();
        missile.owner = player;
        missile.GetComponent<Rigidbody2D>().velocity = Vector3.up * missile.GetComponent<Missile>().speed;
        //AudioSource.PlayClipAtPoint(shootSound, missile.transform.position);
    }

}

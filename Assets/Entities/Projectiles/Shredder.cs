using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Shredder : MonoBehaviour {

	// Destroy objects e.g bullets when the enter shredder
	// outside screen
	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.tag != "Player" && collider.gameObject.tag != "Unshreddable")
            NetworkServer.Destroy(collider.gameObject);
	}
}

using UnityEngine;
using System.Collections;

public class Shredder : MonoBehaviour {

	// Destroy objects e.g bullets when the enter shredder
	// outside screen
	void OnTriggerEnter2D(Collider2D collider) {
		Destroy (collider.gameObject);
	}
}

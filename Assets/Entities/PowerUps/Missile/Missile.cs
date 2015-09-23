using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Missile : Projectile {

    public GameObject bigExplosionPrefab;
    private Vector3 direction;
    private GameObject target;


    // Use this for initialization
    void Start () {
        direction = GetComponent<Rigidbody2D>().velocity.normalized;
        StartCoroutine(IncreaseSpeed());
	}
	
	// Update is called once per frame
	void Update () {
        // Lock on to an enemy target
        FindTarget();

        // Rotate towards target
        if (target) {
            direction = Vector3.Normalize(target.transform.position - transform.position);
            float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, (rotationZ)),1);
        }

    }

    // Find an enemy to lock-on
    void FindTarget() {
        if (!target) {
            target = GameObject.FindGameObjectWithTag("Enemy");
        }
    }

    // Increase speed greatly after about a second
    IEnumerator IncreaseSpeed() {
        yield return new WaitForSeconds(0.8f);
        GetComponent<Rigidbody2D>().velocity = direction * 30f;
    }

    // When it hits the enemy, it generates a big damaging explosion
    public override void Hit() {
        GameObject bigExplosion = Instantiate(bigExplosionPrefab,transform.position, Quaternion.identity) as GameObject;
        bigExplosion.GetComponent<Projectile>().owner = owner;
        //if(NetworkServer.active) NetworkServer.Spawn(bigExplosion);
        Destroy(gameObject);
    }
}

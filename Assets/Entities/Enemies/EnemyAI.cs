using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyAI : NetworkBehaviour {
	public float health = 100f;
	public GameObject projectile;
	public float projectileShootRate = 1f;
	public int scoreValue = 150;
	public float rotationSpeed = 8f;
    public AudioClip shootSound;
    public bool isAlive = true;

	private float pos;
    private GameObject player;
    private PlayerController killer;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("SetPlayer", 0.0001f, Random.Range(2.0f, 4.0f));
        pos = transform.position.x;


        // Random vertical speed
        transform.GetComponent<Rigidbody2D>().drag = Random.Range(5, 10);

    }

    // Change target player from time to time
    [ServerCallback]
    void SetPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int count = players.GetLength(0);
        int id = Random.Range(0, count);
        if (count > 0) {
            player = players[id];
        }
    }


 
    void Update()
    {
        if (!isServer) return;
        // Rotate towards player
        if (player) {
            Vector3 dir = transform.position - player.transform.position;
            dir.Normalize();
            float rotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, (rotationZ - 90)), Time.deltaTime * rotationSpeed);
        }
        else {
            SetPlayer();
        }

        // Shoot at player
        float probability = projectileShootRate * Time.deltaTime;
        bool alive = player.GetComponent<PlayerController>().isAlive;
        if (Random.value < probability && alive == true) {
            RpcShoot();
        }
        MoveEnemyPosition();
    }

    // Just shoot a bullet towards the player
    [ClientRpc]
    void RpcShoot() {
		if(!player) return;
		Vector3 bulletPos = transform.position;
        bulletPos += transform.rotation * (0.5f* Vector3.down); 
		GameObject bullet = Instantiate(projectile, bulletPos, transform.rotation) as GameObject;
		Vector3 direction = transform.rotation * Vector3.down;
		direction.Normalize();
		bullet.GetComponent<Rigidbody2D>().velocity = direction * bullet.GetComponent<Projectile>().speed;
        AudioSource.PlayClipAtPoint(shootSound, bullet.transform.position);

    }



    [ServerCallback]
    void OnTriggerEnter2D(Collider2D collider) {
        Projectile bullet = collider.gameObject.GetComponent<Projectile>();
        float damage = 0;
        if (bullet) {
            killer = bullet.owner.GetComponent<PlayerController>();
            damage = bullet.GetDamage();
            bullet.Hit();
            RpcDamaged(damage);
        }
    }

    [ClientRpc]
    void RpcDamaged(float damage) {
        health -= damage;
        // hit effect
        GameObject hit = Instantiate(Resources.Load("YellowBulletHit"), transform.position, Quaternion.identity) as GameObject;
        hit.transform.parent = transform;
        Destroy(hit, 0.9f);
        if (!isAlive) return;
        if (health <= 0) {
            if(isServer && killer) killer.RpcAddScore(scoreValue);
            Die();
        }
    }
	
	void Die(){
		GameObject explosion = Instantiate(Resources.Load("Explosion"), transform.position, Quaternion.identity) as GameObject;
		isAlive = false;
		Destroy (explosion,1f);
		Destroy(gameObject);
	}

	
	// Moves the enemy from right to left
	void MoveEnemyPosition(){
		pos += Time.deltaTime;
		transform.position = new Vector3(Mathf.PingPong(pos, 4.0f) - 2.0f, transform.position.y, transform.position.z);
	}
	
}


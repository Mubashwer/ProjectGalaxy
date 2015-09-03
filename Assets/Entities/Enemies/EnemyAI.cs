using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyAI : NetworkBehaviour {
	public float health = 100f;
	public GameObject projectile;
	public float projectileSpeed = 10f;
	public float projectileShootRate = 1f;
	public int scoreValue = 150;
	public float rotationSpeed = 8f;
    public AudioClip shootSound;
    public bool isAlive = true;

	private float pos;
    [SyncVar]
    private GameObject player;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("SetPlayer", 0.0001f, Random.Range(2.0f, 4.0f));

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


    [ServerCallback]
    void Update()
    {
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
        if (Random.value < probability) {
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
		GameObject instantiatedProjectile = Instantiate(projectile, bulletPos, transform.rotation) as GameObject;
		Vector3 direction = transform.rotation * Vector3.down;
		direction.Normalize();
		instantiatedProjectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        AudioSource.PlayClipAtPoint(shootSound, instantiatedProjectile.transform.position);
        NetworkServer.Spawn(instantiatedProjectile);

    }
	

	void OnTriggerEnter2D(Collider2D collider){
		Projectile playerProjectile = collider.gameObject.GetComponent<Projectile>();
		if(playerProjectile){
			playerProjectile.Hit ();
			GameObject hit = Instantiate(Resources.Load("YellowBulletHit"), transform.position, Quaternion.identity) as GameObject;
			hit.transform.parent = transform;
			Destroy(hit, 0.9f);
			if(!isAlive) return;
			health -= playerProjectile.GetDamage();
			if (health <= 0) {
				Die();
                playerProjectile.owner.GetComponent<PlayerController>().addScore(scoreValue);
			}
		}
	}
	
	void Die(){
		GameObject explosion = Instantiate(Resources.Load("Explosion"), transform.position, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(explosion);
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


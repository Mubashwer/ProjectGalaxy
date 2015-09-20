using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	public float health = 100f;
	public GameObject projectile;
	public float projectileSpeed = 10f;
	public float projectileShootRate = 1f;
	public int scoreValue = 150;
	public float rotationSpeed = 8f;
    public AudioClip shootSound;

	private ScoreKeeper scoreKeeper;
	private GameObject player;
	public bool isAlive = true;
	private float pos; 
	
	void Start() {
		player = GameObject.Find ("Player");
		scoreKeeper = GameObject.Find ("Score").GetComponent<ScoreKeeper>();
		
		// Initiate random starting position
		pos = Random.Range(-2f, 2f);
		transform.position = new Vector3(pos , transform.position.y, transform.position.z);
		
		// Random vertical speed
		transform.GetComponent<Rigidbody2D>().drag = Random.Range(5,10);
		
	}
	
	void Update(){
		
		// Rotate towards player
		if(player){
			Vector3 dir = transform.position - player.transform.position;
			dir.Normalize();
			float rotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, (rotationZ - 90)), Time.deltaTime * rotationSpeed);
		}
		// Shoot at player
		float probability = projectileShootRate * Time.deltaTime;
		bool alive = player.GetComponent<PlayerController>().isAlive;
		if(Random.value < probability && alive == true ){
			Shoot ();
		}
		
		MoveEnemyPosition();
		
		
	}
	
	// Just shoot a bullet towards the player
	void Shoot() {
		if(!player) return;
		Vector3 bulletPos = transform.position;
        bulletPos += transform.rotation * (0.5f* Vector3.down); 
		GameObject instantiatedProjectile = Instantiate(projectile, bulletPos, transform.rotation) as GameObject;
		Vector3 direction = transform.rotation * Vector3.down;
		direction.Normalize();
		instantiatedProjectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        AudioSource.PlayClipAtPoint(shootSound, instantiatedProjectile.transform.position);
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
				scoreKeeper.Score(scoreValue);
				Die();
			}
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


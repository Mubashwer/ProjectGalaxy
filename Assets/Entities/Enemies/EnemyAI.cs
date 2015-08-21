using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	public float health = 100f;
	public GameObject hitEffect;
	public GameObject projectile;
	public float projectileSpeed = 10f;
	public float projectileShootRate = 1f;
	public int scoreValue = 150;
	
	private ScoreKeeper scoreKeeper;
	private GameObject player;
	
	void Start() {
		player = GameObject.Find ("Player");
		scoreKeeper = GameObject.Find ("Score").GetComponent<ScoreKeeper>();
		
	}
	
	void Update(){
		
		// Rotate towards player
		if(player){
			Vector3 dir = transform.position - player.transform.position;
			dir.Normalize();
			float rotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, (rotationZ - 90));
		}
		// Shoot at player
		float probability = projectileShootRate * Time.deltaTime;
		if(Random.value < probability){
			Shoot ();
		}
		
	}
	
	// Just shoot a bullet towards the player
	void Shoot() {
		if(!player) return;
		Vector3 bulletPos = transform.position;
		GameObject instantiatedProjectile = Instantiate(projectile, bulletPos, transform.rotation) as GameObject;
		Vector3 direction = player.transform.position - transform.position;
		direction.Normalize();
		instantiatedProjectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;		
	}
	
	void OnTriggerEnter2D(Collider2D collider){
		Projectile playerProjectile = collider.gameObject.GetComponent<Projectile>();
		if(playerProjectile){
			playerProjectile.Hit ();
			GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
			hit.transform.parent = transform;
			Destroy(hit, 0.9f);
			
			if (health <= 0) {
				Destroy(gameObject);
				
			}
			health -= playerProjectile.GetDamage();
			if (health <= 0) {
				scoreKeeper.Score(scoreValue);
				Die();
			}
		}
	}
	
	void Die(){
		Destroy(gameObject);
	}
}


using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BossAI : NetworkBehaviour {
	public float health; 
	public GameObject projectile;
	public float projectileShootRate = 1f;
	public GameObject minion;
	public int scoreValue; 
	public bool isAlive = false;
	
	private PlayerController killer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
}

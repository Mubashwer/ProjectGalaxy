using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController : NetworkBehaviour {

	//  Variables for restricting movement
	
	public float maxHealth = 200f;
	public GameObject projectile;
	public float projectileShootRate = 0.15f;
	public GameObject hitEffect;
    public AudioClip shootSound;
    public bool isAlive = true;
	public Sprite mySprite;

    public PowerUpItem item;
    public PowerUp powerUp;

    private float xMin, xMax, yMin, yMax, padding = 0.5f;
    [SyncVar]
    private float health; // current health
    [SyncVar]
    private float score = 0;

   



    public override void OnStartClient() {
        base.OnStartClient();

        // CHange colour and start position of second player
        if (GameObject.FindGameObjectsWithTag("Player").Length > 1) { 
			GetComponent<SpriteRenderer>().sprite = mySprite;
			transform.position = GameObject.Find("StartPosition2").transform.position;
        }
        	

    }

    // Use this for initialization
    void Start () {
		
		float distFromCam = transform.position.z - Camera.main.transform.position.z;
		xMin = Camera.main.ViewportToWorldPoint (new Vector3(0,0,distFromCam)).x+padding;
		xMax = Camera.main.ViewportToWorldPoint (new Vector3(1,0,distFromCam)).x-padding;
		yMin = Camera.main.ViewportToWorldPoint (new Vector3(0,0,distFromCam)).y+padding;
		yMax = Camera.main.ViewportToWorldPoint (new Vector3(1,1,distFromCam)).y-padding;
		health = maxHealth;
        DontDestroyOnLoad(gameObject);
	}
	
	
	// Update is called once per frame
	void Update() {
        if (!isLocalPlayer) {
            return;
        }
        // Tapping Screen   
        if (Input.touchCount > 0) {

            //Shoot regular bullet at a fixed rate for touching screen
            if (Input.touches[0].phase == TouchPhase.Began) {
                StartCoroutine(KeepShooting());
            }
            if (Input.touches[0].phase == TouchPhase.Ended) { 
                StopAllCoroutines();
            }

            // If player has double tap shooting powerUp, then shoot it
            for (int i = 0; i < Input.touchCount; i++) {
                Touch touch = Input.GetTouch(i);
                if(touch.phase == TouchPhase.Began) {
                    if (touch.tapCount >= 2 && powerUp && powerUp.doubleTap) {
                        CmdDoubleTap();
                        powerUp.CountDown();
                    }
                }
            }    
        }

        // Left-alt for powerUp shot, space for regular shot
        if (Input.GetKeyDown(KeyCode.LeftAlt) && powerUp && powerUp.doubleTap) {
            CmdDoubleTap();
            powerUp.CountDown();
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(KeepShooting());
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            StopAllCoroutines();
        }



        // Follow touch swipe or mouse left-click
        FollowSwipe();

	}

    IEnumerator KeepShooting()
    {
        while (true) {
            CmdShoot();
            yield return new WaitForSeconds(projectileShootRate);
        }
    }

    public float getHealth() {
		return health;
	}

    //Detects collision in server but updates damage in all clients
    [ServerCallback]
    void OnTriggerEnter2D(Collider2D collider){

        Projectile bullet = collider.gameObject.GetComponent<Projectile>();
        float damage = 0;
		if(bullet){
            bullet.Hit();
            damage = bullet.GetDamage();
            if (item && powerUp && powerUp.isActivated() && powerUp.isDefensive) {
                damage = powerUp.Defend(damage);
            }
            RpcDamaged(damage);
		}
	}
    
    // Takes damage and (maybe?) dies in all clients
    [ClientRpc]
    void RpcDamaged(float damage) {
        health -= damage;
        // hit effect
        GameObject hit = Instantiate(Resources.Load("YellowBulletHit"), transform.position, Quaternion.identity) as GameObject;
        hit.transform.parent = transform;
        Destroy(hit, 0.9f);
        if (!isAlive) return;
        if (health <= 0) {
            Die();
        }
    }

    // Transfers to server and then RpcShoot() is called on all clients to generate local bullets
    [Command]
    void CmdShoot() {
        RpcShoot();
    }

    // Shoots unsyncrhonized bullets in all clients
    [ClientRpc]
    void RpcShoot() {
        Vector3 bulletPos = transform.position;
        bulletPos.y += 0.5f;
        GameObject bullet = Instantiate(projectile, bulletPos, Quaternion.identity) as GameObject;
        bullet.GetComponent<Projectile>().owner = gameObject;
        bullet.GetComponent<Rigidbody2D>().velocity = Vector3.up * bullet.GetComponent<Projectile>().speed;
        AudioSource.PlayClipAtPoint(shootSound, bullet.transform.position);
    }


    //Transfer to server and then run double tap on all clients
    [Command]
    void CmdDoubleTap() {
        RpcDoubleTap();
    }

    //Run double tap powerUp method on all clients
    [ClientRpc]
    void RpcDoubleTap() {
        powerUp.DoubleTapEvent();
    }


    // Functions called by local powerUp HUD to destroy item and powerup
    [Command]
    public void CmdDestroyPowerUpItem() {
        if (item && item.isServer) NetworkServer.Destroy(item.gameObject);
        if (item) Destroy(item.gameObject);
    }

    [ClientRpc]
    public void RpcDestroyPowerUp() {
        if (item) Destroy(item);
        if (powerUp) Destroy(powerUp);
    }



    // Move with same velocity as touch swipe
    private void FollowSwipe() {
		if(Input.touchCount > 0) { 
			// Check for any touch swipe
			for(int i = 0; i < Input.touchCount && Input.touches[i].phase == TouchPhase.Moved; i++) {
				Vector2 delta = Input.touches[i].deltaPosition; // touch displacement
				Vector3 delta3 = new Vector3(delta.x, delta.y, transform.position.z);
				// move in the direction and distance of touch displacement in world space
				transform.Translate(delta3 * Time.deltaTime, Space.World);
			}
		} 
		// otherwise move to left-click position of mouse
		else if (Input.touchCount <= 0 && Input.GetMouseButton(0)){
			Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
			transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
		}
		
		// Clamp/Restrict the player to the play space
		float newX = Mathf.Clamp (transform.position.x, xMin, xMax);
		float newY = Mathf.Clamp (transform.position.y, yMin, yMax);
		transform.position = new Vector3(newX, newY, transform.position.z);

	}


    // Updates scores in all clients
    [ClientRpc]
    public void RpcAddScore(float points) {
        score += points;
    }

    public float getScore() {
        return score;
    }

    void Die(){
		GameObject explosion = Instantiate(Resources.Load("Explosion"), transform.position, Quaternion.identity) as GameObject;
		isAlive = false;
        Destroy(explosion,1f);
		Destroy(gameObject);
	
	}
	
}

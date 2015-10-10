﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController : NetworkBehaviour {

	//  Variables for restricting movement
	public float maxHealth = 200f;
	public float projectileShootRate = 0.15f;
    public AudioClip shootSound;
    public bool isAlive = true;
	public Sprite mySprite;
	public GameObject Player;
	public Renderer myRenderer;
	public int lives = 3;

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
        var players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 1) {
            transform.position = new Vector3(-1f, -3.5f, 0f);
            print(transform.position);
                
        }
        else if (players.Length > 1) { 
			GetComponent<SpriteRenderer>().sprite = mySprite;
			transform.position = GameObject.Find("StartPosition2").transform.position;
        }

        SetDirtyBit(1);	

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
        myRenderer = GetComponent<Renderer>();
        myRenderer.enabled = true;
        Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	
	// Update is called once per frame
	void Update() {
        if (!isLocalPlayer || isAlive == false) {
            return;
        }
        print(transform.position);
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
	
	public void increaseLives(){
		lives++;
	}

    //Detects collision in server but updates damage/powerUp in all clients
    [ServerCallback]
    void OnTriggerEnter2D(Collider2D collider){
        if (!isAlive) return;
        Projectile bullet = collider.gameObject.GetComponent<Projectile>();
        PowerUpItem newItem = collider.gameObject.GetComponent<PowerUpItem>();
        float damage = 0;

        if (bullet){
            bullet.Hit(); //destroy bullet
            damage = bullet.GetDamage(); // get damage
            if (powerUp && powerUp.isActivated() && powerUp.isDefensive) {
                damage = powerUp.Defend(damage); // update damage via defensive powerUp
            }
            RpcDamaged(damage); // take damage in all clients
		}

        if (newItem) {
            newItem.GetComponent<Rigidbody2D>().isKinematic = true; // will stop moving
            newItem.GetComponent<Collider2D>().enabled = false; // will no longer collide

            if (powerUp && powerUp.GetId() != newItem.GetId()) {
                RpcPowerUpWrapUp(); //destroy existing powerUp
            }

            RpcPowerUpExtract(newItem.powerUpName, newItem.GetId()); //extract powerUp in all clients
            NetworkServer.Destroy(newItem.gameObject); // Destroy item in all clients
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

    // Extract powerUp in all clients
    [ClientRpc]
    void RpcPowerUpExtract(string name, int id) {
        GameObject powerUpObject = Instantiate(Resources.Load(name + "PowerUp")) as GameObject;
        powerUp = powerUpObject.GetComponent<PowerUp>();
        powerUp.Setup(gameObject, id);
    }

    // Destroy powerUp in all clients
    [ClientRpc]
    void RpcPowerUpWrapUp() {
        if(powerUp) powerUp.WrapUp(); 
    }

    // This is called when client player connects. Existing powerUp of host player
    // is loaded in client scene and configured.
    [ClientRpc]
    public void RpcPowerUpReSetup() {
        if (isLocalPlayer) return;
        PowerUp[] powerUps = FindObjectsOfType<PowerUp>();
        foreach(PowerUp p in powerUps) {
            powerUp = p;
            powerUp.SetPlayer(gameObject);
            powerUp.ReplacePlayerSprite();
            return;
        }
        return;
    }


    // Transfers to server and then RpcShoot() is called on all clients to generate local bullets
    [Command]
    void CmdShoot() {
        RpcShoot();
    }

    // Shoots unsyncrhonized bullets in all clients
    [ClientRpc]
    void RpcShoot() {
        if(powerUp && powerUp.shootChange) {
            powerUp.Shoot();
            return;
        }
        Vector3 bulletPos = transform.position;
        bulletPos.y += 0.5f;
        GameObject bullet = Instantiate(Resources.Load("YellowBullet"), bulletPos, Quaternion.identity) as GameObject;
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


    /*// Functions called by local powerUp HUD to destroy item and powerup
    [Command]
    public void CmdDestroyPowerUpItem() {
        if (item && item.isServer) NetworkServer.Destroy(item.gameObject);
        if (item) Destroy(item.gameObject);
    }*/

    // Move with same velocity as touch swipe
    private void FollowSwipe() {
		if(Input.touchCount > 0) { 
			// Check for any touch swipe
			for(int i = 0; i < Input.touchCount && Input.touches[i].phase == TouchPhase.Moved; i++) {
				Vector2 delta = Input.touches[i].deltaPosition; // touch displacement
				Vector3 delta3 = new Vector3(delta.x, delta.y, transform.position.z);
				// move in the direction and distance of touch displacement
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
		float newY = Mathf.Clamp (transform.position.y, yMin, yMax - 1.2f);
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
        if (powerUp) powerUp.WrapUp();
        Destroy(explosion,1f);
		lives--;
        StopAllCoroutines();
        if (lives <= 0) {
            Respawn(); // TODO: GAME OVER screen    
        }
        else {
			Respawn();	
		}

	
	}
	IEnumerator Blink(){
		yield return new WaitForSeconds(2);
		for(int i = 0;i<10;i++){
			myRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
			myRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
		}
		//renderer.enabled = true;
		isAlive = true;
        transform.FindChild("RightExhaustFlames").gameObject.GetComponent<Renderer>().enabled = true;
        transform.FindChild("LeftExhaustFlames").gameObject.GetComponent<Renderer>().enabled = true;
    }
	
	void Respawn(){
		myRenderer.enabled = false;
        transform.FindChild("RightExhaustFlames").gameObject.GetComponent<Renderer>().enabled = false;
        transform.FindChild("LeftExhaustFlames").gameObject.GetComponent<Renderer>().enabled = false;
        StartCoroutine(Blink ());
		health = maxHealth;

        //isAlive = true;
    }
}

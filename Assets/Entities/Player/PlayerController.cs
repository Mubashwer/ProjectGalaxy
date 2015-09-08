using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController : NetworkBehaviour {

	//  Variables for restricting movement
	
	public float maxHealth = 200f;
	public GameObject projectile;
	public float projectileSpeed = 10f;
	public float projectileShootRate = 0.15f;
	public GameObject hitEffect;
    public AudioClip shootSound;
    public bool isAlive = true;
	public Sprite mySprite;

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
        if (!isLocalPlayer) return;
        // Fire bullet at a fixed rate if screen is touched or space is pressed
        if ((Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) ||
			Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(KeepShooting(gameObject));
        }
		if(((Input.touchCount > 0 && (Input.touches[0].phase == TouchPhase.Ended ||
			Input.touches[0].phase == TouchPhase.Canceled) )) || 
			Input.GetKeyUp(KeyCode.Space)) {
            StopAllCoroutines();
        }
		// Follow touch swipe or mouse left-click
		FollowSwipe ();

	}
    IEnumerator KeepShooting(GameObject shooter)
    {
        while (true) {
            yield return new WaitForSeconds(projectileShootRate);
            CmdShoot();
        }
    }

    public float getHealth() {
		return health;
	}


    void OnTriggerEnter2D(Collider2D collider){
        Projectile enemyProjectile = collider.gameObject.GetComponent<Projectile>();
		if(enemyProjectile){
			health -= enemyProjectile.GetDamage();
			enemyProjectile.Hit();
			 // hit effect
			GameObject hit = Instantiate(Resources.Load("YellowBulletHit"), transform.position, Quaternion.identity) as GameObject;
            hit.transform.parent = transform;
            NetworkServer.Spawn(hit);
            Destroy(hit, 0.9f);
			if(!isAlive) return; 
			if (health <= 0) {
				Die ();
			}
		}
	}

    // Just shoot a bullet
    [Command]
    void CmdShoot()
    {
        Vector3 bulletPos = transform.position;
        bulletPos.y += 0.5f;
        GameObject instantiatedProjectile = Instantiate(projectile, bulletPos, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(instantiatedProjectile);
        instantiatedProjectile.GetComponent<Projectile>().owner = gameObject;
        instantiatedProjectile.GetComponent<Rigidbody2D>().velocity = Vector3.up * projectileSpeed;
        AudioSource.PlayClipAtPoint(shootSound, instantiatedProjectile.transform.position);
        
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

    public void addScore(float points) {
        score += points;
    }

    public float getScore() {
        return score;
    }

    void Die(){
		GameObject explosion = Instantiate(Resources.Load("Explosion"), transform.position, Quaternion.identity) as GameObject;
		isAlive = false;
        NetworkServer.Spawn(explosion);
        Destroy(explosion,1f);
		Destroy(gameObject);
	
	}
	
}

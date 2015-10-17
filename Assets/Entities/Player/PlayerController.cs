using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController : NetworkBehaviour {

    //  Variables set in inspector
    public float[] maxHealth;
    public float[] projectileShootRate;
   
    public int[] lives;
    public Sprite alternateSprite;
    public AudioClip shootSound;


    public bool IsAlive { get; set; }
    public int PlayerNum { get; set; }
    [HideInInspector]
    public PowerUpItem item;
    [HideInInspector]
    public PowerUp powerUp;

    private Renderer myRenderer;
    private float xMin, xMax, yMin, yMax, padding = 0.5f; // for clamping player movement
    [SyncVar]
    private float health;
    [SyncVar]
    private float score = 0;
    [SyncVar]
    private int currentLives;
    private int difficulty = 1; // 0 = Easy, 1 = Normal, 2 = Hard (manually synced)

    private bool difficultySynced = false;



    public override void OnStartClient() {
        base.OnStartClient();

        PlayerNum = GameObject.FindGameObjectsWithTag("Player").Length;

        // CHange colour and start position of second player
        if (PlayerNum > 1) {
            GetComponent<SpriteRenderer>().sprite = alternateSprite;
            transform.position = GameObject.Find("StartPosition2").transform.position;
        }
        SetDirtyBit(1);

    }

    void Awake() {
        IsAlive = true;
    }

    // Use this for initialization
    void Start() {

        float distFromCam = transform.position.z - Camera.main.transform.position.z;
        xMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distFromCam)).x + padding;
        xMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distFromCam)).x - padding;
        yMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distFromCam)).y + padding;
        yMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, distFromCam)).y - padding - 1.2f;

        difficulty = (int)GameManager.instance.CurrentPlayerDifficulty;
        health = maxHealth[difficulty];
        if(isLocalPlayer) {
            if (!difficultySynced) {
                CmdSyncDifficulty(difficulty);
                difficultySynced = true;
            }
        }

        DontDestroyOnLoad(gameObject);
        myRenderer = GetComponent<Renderer>();
        myRenderer.enabled = true;
    }


    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer || IsAlive == false) {
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
                if (touch.phase == TouchPhase.Began) {
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

    [Command]
    void CmdSyncDifficulty(int difficulty) {
        RpcSyncDifficulty(difficulty);
    }
    [ClientRpc]
    void RpcSyncDifficulty(int difficulty) {
        this.difficulty = difficulty;
        health = maxHealth[difficulty];
        currentLives = lives[difficulty];
    }

    IEnumerator KeepShooting() {
        while (true) {
            CmdShoot();
            yield return new WaitForSeconds(projectileShootRate[difficulty]);
        }
    }

    public float GetMaxHealth() {
        return maxHealth[difficulty];
    }

    public float getHealth() {
        return health;
    }

    //Detects collision in server but updates damage/powerUp in all clients
    [ServerCallback]
    void OnTriggerEnter2D(Collider2D collider) {
        if (!IsAlive) return;
        Projectile bullet = collider.gameObject.GetComponent<Projectile>();
        PowerUpItem newItem = collider.gameObject.GetComponent<PowerUpItem>();
        float damage = 0;

        if (bullet) {
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
        if (!IsAlive) return;
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
        if (powerUp) powerUp.WrapUp();
    }

    // This is called when client player connects. Existing powerUp of host player
    // is loaded in client scene and configured.
    [ClientRpc]
    public void RpcPowerUpReSetup() {
        if (isLocalPlayer) return;
        PowerUp[] powerUps = FindObjectsOfType<PowerUp>();
        foreach (PowerUp p in powerUps) {
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
        if (powerUp && powerUp.shootChange) {
            powerUp.Shoot();
            AudioSource.PlayClipAtPoint(shootSound, transform.position, 0.9f);
            return;
        }
        Vector3 bulletPos = transform.position;
        bulletPos.y += 0.5f;
        GameObject bullet = Instantiate(Resources.Load("YellowBullet"), bulletPos, Quaternion.identity) as GameObject;
        bullet.GetComponent<Projectile>().owner = gameObject;
        bullet.GetComponent<Rigidbody2D>().velocity = Vector3.up * bullet.GetComponent<Projectile>().speed;
        AudioSource.PlayClipAtPoint(shootSound, bullet.transform.position, 0.9f);
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


    // Move with same velocity as touch swipe
    private void FollowSwipe() {
        if (Input.touchCount > 0) {
            // Check for any touch swipe
            for (int i = 0; i < Input.touchCount && Input.touches[i].phase == TouchPhase.Moved; i++) {
                Vector2 delta = Input.touches[i].deltaPosition; // touch displacement
                Vector3 delta3 = new Vector3(delta.x, delta.y, transform.position.z);
                // move in the direction and distance of touch displacement
                transform.Translate(delta3 * Time.deltaTime, Space.World);
            }
        }
        // otherwise move to left-click position of mouse
        else if (Input.touchCount <= 0 && Input.GetMouseButton(0)) {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        }

        // Clamp/Restrict the player to the play space
        float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
        float newY = Mathf.Clamp(transform.position.y, yMin, yMax);
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

    void Die() {
        // Explosion effect
        GameObject explosion = Instantiate(Resources.Load("Explosion"), transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, 1f);

        IsAlive = false;
        if (powerUp) powerUp.WrapUp(); //Remove any PowerUp

        currentLives--; //Use up life
        StopAllCoroutines();

        myRenderer.enabled = false;
        transform.FindChild("RightExhaustFlames").gameObject.GetComponent<Renderer>().enabled = false;
        transform.FindChild("LeftExhaustFlames").gameObject.GetComponent<Renderer>().enabled = false;
        if (currentLives <= 0) {
            if (isLocalPlayer) LevelManager.instance.GameOver("you are dead", (int)score);
        }
        else {
            Respawn();
        }
        

    }


    IEnumerator Blink() {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < 10; i++) {
            myRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            myRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        //renderer.enabled = true;
        IsAlive = true;
        transform.FindChild("RightExhaustFlames").gameObject.GetComponent<Renderer>().enabled = true;
        transform.FindChild("LeftExhaustFlames").gameObject.GetComponent<Renderer>().enabled = true;
    }

    void Respawn() {
        StartCoroutine(Blink());
        health = maxHealth[difficulty];
    }
    
    [Command]
    public void CmdSetSpawners(bool value) {
        EnemyController.instance.Enabled = value;
        PowerUpController.instance.Enabled = value;
    }

    [Command]
    public void CmdSetPlayerDifficulty(GameManager.Difficulty difficulty) {
        GameManager.instance.CurrentPlayerDifficulty = difficulty;
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        if (GameObject.Find("CanvasGameOver")) return;
        LevelManager.instance.GameOver("host disconnected", (int)score);
    }
   
}

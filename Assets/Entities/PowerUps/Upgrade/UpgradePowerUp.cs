using UnityEngine;
using System.Collections;

public class UpgradePowerUp : PowerUp {


	// Use this for initialization
	void Start () {   
	}
	
	// Update is called once per frame
	void Update () {
        if (timerStarted) CountDownTime();
    }

    public override void Shoot() {
        Vector3 bulletPos = player.transform.position;
        bulletPos.y += 0.5f;

        // Shoot a bigger bullet blaster
        GameObject bullet = Instantiate(Resources.Load("YellowBulletBlaster"), bulletPos, Quaternion.identity) as GameObject;
        bullet.GetComponent<Projectile>().owner = player;
        bullet.GetComponent<Rigidbody2D>().velocity = Vector3.up * bullet.GetComponent<Projectile>().speed;

        // Shoot 4 long bullets
        for(int i = 0; i < 4; i++) {
            bulletPos = player.transform.position;

            if(i == 0) {
                bulletPos.x -= 1.083f;
            }
            if(i == 1) {
                bulletPos.x -= 1.158f;
            }
            if(i == 2) {
                bulletPos.x += 0.53f;
            }
            else {
                bulletPos.x += 0.605f;
            }
            bulletPos.y += 0.5f;
            bullet = Instantiate(Resources.Load("YellowLongBullet"), bulletPos, Quaternion.identity) as GameObject;
            bullet.GetComponent<Projectile>().owner = player;
            bullet.GetComponent<Rigidbody2D>().velocity = Vector3.up * bullet.GetComponent<Projectile>().speed;
        }        
        //AudioSource.PlayClipAtPoint(shootSound, bullet.transform.position);
    }

    public override void Setup(GameObject player, int id) {
        base.Setup(player, id);
        ReplacePlayerSprite();
    }

    public override void WrapUp() {
        RestorePlayerSprite();
        base.WrapUp();
    }

    public override void OnNetworkDestroy() {
        RestorePlayerSprite();
        base.OnNetworkDestroy();
    }
}

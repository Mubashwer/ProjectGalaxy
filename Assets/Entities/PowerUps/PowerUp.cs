﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PowerUp : NetworkBehaviour {

    protected GameObject player; // player who received the power-up

    protected bool activated; // checks whether power-up has been activated or not
    protected bool deactivated; // checks whether power-up is deactivated after being activated or not

    public bool hasTimer; // checks whether the power-up takes effect for a limited time
    public bool instantActivation; // checks to see whether a double-tap is needed to activate it or not
    public bool isDefensive; // checks to see if the power-up is defensive or not
    public bool isOffensive;// checks to see if the power-up is offensive or not

    public bool spriteChange; // checks to see whether the player changes sprite
    public Sprite playerOldSprite; // stores old player sprite

    public bool isPermanent; // checks to see whether the powerup is permanent for the duration of game or not
    public float duration; // duration of effect to last in seconds

    // Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
    }


    // Installs the powerup
    public virtual void Setup() {

    }


    // Replaces shooting for player
    [Command]
    public virtual void CmdShoot() {
    }


    // Replaces collision detection for player
    public virtual void Defend(Collider2D collider) {
    }

    // Do stuffs before removing powerup
    public virtual void WrapUp() {
        Destroy(gameObject);
    }


    public GameObject GetPlayer() {
        return player;
    }

    public void SetPlayer(GameObject player) {
        this.player = player;
    }

    public bool isActivated() {
        return activated;
    }

    public void SetActivated(bool activated) {
        this.activated = activated;
    }

    public bool isDectivated() {
        return activated;
    }

    public void SetDectivated(bool activated) {
        this.activated = activated;
    }

}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDischargeMovement : MonoBehaviour
{
    public Rigidbody2D rb; //We use this in able to apply physics to our object
    public GameObject Player; //A reference to our player game object

    float baseSpeed; //The default speed when we shoot

    Vector2 xscale; //Our x scale, which we'll use to flip the image to have face right or left.

    private bool start = true; //Start delay boolean

    IEnumerator delayCoroutine; //Delay coroutine 

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        xscale = gameObject.transform.localScale;
        xscale.x = Player.transform.localScale.x;
        gameObject.transform.localScale = xscale;

        baseSpeed = Magic_Discharge.buffSpeed; //The basespeed will be modified depend on the level

        delayCoroutine = Delay(1f); //delay by 1 second
    }
    void Start()
    {

        //Whenever we are shooting, our point of fire will be rotating around the player
        //As we move forward, there are times when the bullets are closer to each other
        //whenever there'a max or min of our point of fire's movement (below the player, or above the player)
        //Through this set of code, we prevent that from happening
        switch (Player.transform.localScale.x)
        {
            case 1:
                if (Mathf.Sign(Mathf.Cos(Magic_Movement.angle)) == -1)
                {
                    baseSpeed = Magic_Discharge.buffSpeed;
                }
                else
                {
                    baseSpeed += 5;
                }
                break;
            case -1:
                if (Mathf.Sign(Mathf.Cos(Magic_Movement.angle)) == 1)
                {
                    baseSpeed = Magic_Discharge.buffSpeed;
                }
                else
                {
                    baseSpeed += 5;
                }
                break;
        }

        //The projectory of our "bullet"
        rb.velocity = transform.right * baseSpeed * Player.transform.localScale.x;

        StartCoroutine(delayCoroutine);
    }

    void Update()
    {
        Debug.Log("Buff Speed is currently " + baseSpeed);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //If we hit an enemy, destory itself and the object that it collises with.
        if (col.gameObject.tag == "Enemy")
        {
            Destroy(col.gameObject); //Destroys enemy
            Destroy(gameObject); //Destroys self
        }
    }

    IEnumerator Delay(float time)
    {
        start = true;
        yield return new WaitForSeconds(time);
        Destroy(gameObject); //Destroys self after the yield time
        start = false;
    }
}


    
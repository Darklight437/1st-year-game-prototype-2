using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour {
    //each type of high level audio file
    public AudioSource idleAwake;
    public AudioSource idleAsleep;
    public AudioSource walk;
    public AudioSource EndWalk;
    public AudioSource attack;
    public AudioSource takeDamage;
    public AudioSource TilePlace;
    public AudioSource TileAction;

    float idleTimer = 0;
    
	// Use this for initialization
	void Start ()
    {
        //playsound(idleAwake);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //idleTimer -= Time.deltaTime;

        //if (idleTimer <= 0f && idleAwake.loop == true)
        //{
        //    IdleEnd();
        //}
	}


    //plays the param sound
    public void UsedIdle()
    {

        idleAwake.Play();


    }

    public void activeIdle()
    {

        idleAwake.Play();
        idleAwake.loop = true;
        //idleTimer += 12;
        
    }
    public void IdleEnd()
    {
        idleAwake.loop = false;
        idleAwake.Stop();
    }
       
    public void Attack()
    {
        //IdleEnd();
        attack.Play();
    }

    public void Walk()
    {
        idleAwake.Play();
        walk.Play();
    }

    public void PlaceTile()
    {
        TilePlace.Play();
    }
}

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

    float walkTimer = 0;
    bool endedWalk = false;

	// Use this for initialization
	void Start ()
    {
        //playsound(idleAwake);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (walkTimer <= 0 && endedWalk)
        {
            EndWalk.Play();
            endedWalk = false;
        }
        if (endedWalk)
        {
            walkTimer -= Time.deltaTime;
        }
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
    }
    public void IdleEnd()
    {
        idleAwake.loop = false;
        idleAwake.Stop();
        //idleAsleep.Stop();
    }
       
    public void Attack()
    {
        attack.Play();
    }

    public void Walk()
    {
        idleAwake.Play();
        walk.Play();
        endedWalk = false;

    }

    public void PlaceTile()
    {
        TilePlace.Play();
    }

    public void walkEnd()
    {
        walkTimer = 1.0f;
        endedWalk = true;
    }
}

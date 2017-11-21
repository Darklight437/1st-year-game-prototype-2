using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour {
    //each type of high level audio file
    public AudioSource idleAwake;
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
        
	}


    //plays the param sound
    public void UsedIdle()
    {
        //idleAwake.Play();
        if (idleTimer > 0)
        {
            idleAwake.loop = true;
            //idleTimer = 1.2
        }
    }

    public void activeIdle()
    {

    }

    public void Attack()
    {
        attack.Play();
    }
}

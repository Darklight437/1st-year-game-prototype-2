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

    [HideInInspector]
    public Animator Animator;
    
    
    bool IsWalking = false;
    bool idling = false;

	// Use this for initialization
	void Start ()
    {
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //walk just finished
        if (IsWalking && !Animator.GetBool("IsWalking"))
        {
            IsWalking = false;
            if (EndWalk)
            {
                walk.Stop();
                EndWalk.Play();
            }
            else
            {
                walk.Stop();
            }
        }
        if (idling && !Animator.GetBool("ActionsAvailable"))
        {

        }
    }


    //plays the param sound
    public void UsedIdle()
    {
        if (idleAsleep)
        {
            idleAsleep.Play();
        }
        
    }
    public void stopAll()
    {
        idleAwake.Stop();
        idleAwake.Stop();
        walk.Stop();
        attack.Stop();
        if (TilePlace)
        {
            TilePlace.Stop();
        }
        
        
        
    }

    public void activeIdle()
    {

        if (idleAwake)
        {
            idleAwake.Play();
            idleAwake.loop = true;
        }
        
    }
    public void IdleEnd()
    {
        idleAwake.loop = false;
        idleAwake.Stop();
        
    }
       
    public void Attack()
    {
        if (attack)
        {
            attack.Play();
        }
        attack.Play();
    }

    public void Walk()
    {
        if (walk)
        {
            if (idleAwake)
            {
                idleAwake.Play();
            }
            walk.Play();
            IsWalking = true;
        }
        

    }

    public void PlaceTile()
    {
        TilePlace.Play();
    }
    
    public void tileExtra()
    {
        TileAction.Play();
    }

        
   
}

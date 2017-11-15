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


	// Use this for initialization
	void Start ()
    {
        playsound(idleAwake);
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}


    //plays the param sound
    public void playsound(AudioSource Sound)
    {
        Sound.Play();
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDaddy : MonoBehaviour
{
    //tracks the keys that have been pressed
    public string message = "";

    //key to clear the message
    public KeyCode resetKey = KeyCode.Backspace;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //reset
        if (Input.GetKey(resetKey))
        {
            message = "";
        }

        //iterate through all pressable keys, checking for a press
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                message += key.ToString();

            }
        }

        //if the message has been typed correctly
        if (message == "BIGDADDY")
        {
            Debug.Log("BIGDADDY Protocol has been invoked.");

            message = "";

            //find the game manager and tell it to kill the units
            Object.FindObjectOfType<GameManagment>().KillAll();
            //KILL EVERYTHING
        }
	}
}

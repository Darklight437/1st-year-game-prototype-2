using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCreenshot : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown (KeyCode.P))
		{
			ScreenCapture.CaptureScreenshot ("pic.png",2);
		}
		
	}
}

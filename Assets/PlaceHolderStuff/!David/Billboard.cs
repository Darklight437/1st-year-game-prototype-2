using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{


    public Transform lookAtPos = null;
    public RectTransform myTransform = null;
	// Use this for initialization
	void Start ()
    {
        lookAtPos = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void lookAt(Transform position)
    {
        myTransform.rotation = lookAtPos.rotation;
    }
}

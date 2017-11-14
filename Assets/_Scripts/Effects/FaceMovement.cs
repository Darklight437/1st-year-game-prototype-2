using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMovement : MonoBehaviour {

    private Vector3 m_prevPos = Vector3.zero;
    private Vector3 DefaultForward;
    private Transform myTransform;
    
    // Use this for initialization
    void Start () {
        DefaultForward = transform.forward;
        transform = myTransform;
    }
	
	// Update is called once per frame
	void Update () {

        if (m_prevPos != transform.position)
        {
            transform.forward = transform.position - m_prevPos;
        }
        else
        {
            transform.forward = DefaultForward;
        }
        m_prevPos = transform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMovement : MonoBehaviour
{

    private Vector3 m_prevPos = Vector3.zero;
    private Vector3 DefaultForward;
    private Transform myTransform;
    
    // Use this for initialization
    void Start ()
    {
        m_prevPos = transform.position;
        DefaultForward = transform.forward;
        myTransform = transform;
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (m_prevPos != myTransform.position)
        {
            myTransform.forward = myTransform.position - m_prevPos;
        }
        else
        {
            myTransform.forward = DefaultForward;
        }
        m_prevPos = myTransform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMovement : MonoBehaviour
{

    private Vector3 m_prevPos = Vector3.zero;
    private Quaternion defaultOffset = Quaternion.identity;
    private Transform myTransform;
    
    // Use this for initialization
    void Start ()
    {
        m_prevPos = transform.position;
        defaultOffset = transform.rotation;
        myTransform = transform;
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (m_prevPos != myTransform.position)
        {
            myTransform.rotation = Quaternion.LookRotation((myTransform.position - m_prevPos).normalized) * defaultOffset;
        }
        else
        {
            myTransform.rotation = Quaternion.LookRotation(Vector3.forward) * defaultOffset;
        }
        m_prevPos = myTransform.position;
    }
}

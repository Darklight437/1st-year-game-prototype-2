using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMovement : MonoBehaviour
{

    private Vector3 m_prevPos = Vector3.zero;
    private Quaternion originalDirection = Quaternion.identity;
    private Quaternion defaultOffset = Quaternion.identity;
    private Transform myTransform;
    public Vector3 directionOverride = Vector3.zero;
    
    // Use this for initialization
    void Start ()
    {
        m_prevPos = transform.position;
        originalDirection = transform.parent.rotation;
        defaultOffset = transform.localRotation;
        myTransform = transform;
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (directionOverride == Vector3.zero)
        {
            if (m_prevPos != myTransform.position)
            {
                myTransform.rotation = Quaternion.LookRotation((myTransform.position - m_prevPos).normalized) * defaultOffset;
            }
            else
            {
                myTransform.rotation = originalDirection * defaultOffset;
            }
        }
        else
        {
            myTransform.rotation = Quaternion.LookRotation(directionOverride.normalized) * defaultOffset;
        }

        m_prevPos = myTransform.position;
    }
}

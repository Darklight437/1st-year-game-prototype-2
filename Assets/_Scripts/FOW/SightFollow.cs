using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightFollow : MonoBehaviour
{
    private Transform toFollow;

    private Vector3 tempVec;

    private int distanceToUpdate = 1;

	void Start ()
    {
        toFollow = transform.parent;
        transform.parent = null;
	}
	
	void Update ()
    {
        tempVec.x = (int)toFollow.position.x;
        tempVec.y = (int)toFollow.position.y;
        tempVec.z = (int)toFollow.position.z;

        bool check = ((tempVec.x % distanceToUpdate) + ( tempVec.z % distanceToUpdate)) != 0 ? true : false;

        if (check == false)
        {
            transform.position = tempVec;
        }
	}
}

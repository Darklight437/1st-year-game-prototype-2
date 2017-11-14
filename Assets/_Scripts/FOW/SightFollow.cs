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
        tempVec.x = (int)(toFollow.position.x + 0.5f);
        tempVec.y = (int)(toFollow.position.y + 0.5f);
        tempVec.z = (int)(toFollow.position.z + 0.5f);

        bool check = ((tempVec.x % distanceToUpdate) + ( tempVec.z % distanceToUpdate)) != 0 ? true : false;

        if (check == false)
        {
            transform.position = tempVec;
        }
	}
}

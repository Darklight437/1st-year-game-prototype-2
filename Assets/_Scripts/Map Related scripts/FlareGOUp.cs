using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareGOUp : MonoBehaviour
{
    private float m_speed = 10;

	void Update ()
    {
        transform.position += (Vector3.up * Time.deltaTime) * m_speed;
	}
}

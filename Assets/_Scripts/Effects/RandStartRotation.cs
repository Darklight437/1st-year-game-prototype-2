using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandStartRotation : MonoBehaviour 
{
	public List<float> RotationOptions = new List<float>();

	// Use this for initialization
	void Start ()
	{
		RotationOptions.Add (0);
		RotationOptions.Add (90);
		RotationOptions.Add (180);
		RotationOptions.Add (270);

		int Randd = Random.Range (0, RotationOptions.Count);

		Vector3 OldRand = transform.eulerAngles;
		OldRand.y = RotationOptions [Randd];
		transform.eulerAngles = OldRand;		
	}

}

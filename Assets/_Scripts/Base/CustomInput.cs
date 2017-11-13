using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInput : MonoBehaviour
{

    //key-bindings
    public List<KeyCode> ups = new List<KeyCode>();
    public List<KeyCode> downs = new List<KeyCode>();
    public List<KeyCode> lefts = new List<KeyCode>();
    public List<KeyCode> rights = new List<KeyCode>();

    //vector containing information about pressed keys
    public Vector2 keyInput = Vector2.zero;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        keyInput = Vector2.zero;

        //iterate through each KeyCode array to determine if at least one is pressed
        for (int i = 0; i < ups.Count; i++)
        {
            if (Input.GetKey(ups[i]))
            {
                keyInput.y += 1.0f;
                break;
            }
        }

        for (int i = 0; i < downs.Count; i++)
        {
            if (Input.GetKey(downs[i]))
            {
                keyInput.y -= 1.0f;
                break;
            }
        }

        for (int i = 0; i < lefts.Count; i++)
        {
            if (Input.GetKey(lefts[i]))
            {
                keyInput.x -= 1.0f;
                break;
            }
        }

        for (int i = 0; i < rights.Count; i++)
        {
            if (Input.GetKey(rights[i]))
            {
                keyInput.x += 1.0f;
                break;
            }
        }

        keyInput.Normalize();

    }
}

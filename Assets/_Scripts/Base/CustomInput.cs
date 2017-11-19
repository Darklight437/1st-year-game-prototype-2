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

    //remember the mouse position accross multiple loops
    private Vector2 m_prevMousePosition = Vector2.zero;

    //vector containing information about pressed keys
    public Vector2 keyInput = Vector2.zero;
    public Vector2 mouseInput = Vector2.zero;

    //multiplier for mouse movement
    public float mouseSensitivity = 5.0f;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        keyInput = Vector2.zero;
        mouseInput = Vector2.zero;

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

        //get the position of the mouse
        Vector2 mousePosition = Input.mousePosition;

        //if the mouse is being held
        if (Input.GetMouseButton(0))
        {
            //get the delta position of the mouse
            mouseInput = m_prevMousePosition - mousePosition;
            mouseInput *= mouseSensitivity;
        }

        m_prevMousePosition = mousePosition;

    }
}

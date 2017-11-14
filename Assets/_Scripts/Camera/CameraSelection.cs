using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSelection : MonoBehaviour
{
    //automated reference to the manager and input
    private GameManagment manager = null;

    public LayerMask tileLayer;

	// Use this for initialization
	void Start ()
    {
        manager = GameObject.FindObjectOfType<GameManagment>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //detect mouse clicks
		if (Input.GetMouseButtonUp(0))
        {

            if (manager.uiPressed)
            {
                manager.uiPressed = false;
            }
            else
            {


                //get a ray originating from the mouse pointing forwards
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                //data from the raycast
                RaycastHit hitInfo;
                //check if the raycast hit anything
                if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hitInfo, float.MaxValue, tileLayer.value))
                {
                    //get the object that the raycast hit
                    GameObject hitObject = hitInfo.collider.gameObject;

                    //get the tiles component (null if there isn't one)
                    Tiles tiles = hitObject.GetComponent<Tiles>();

                    if (tiles != null && manager.activePlayer.isHuman)
                    {
                        manager.OnTileSelected(tiles);
                    }
                }
            }
        }
	}
}

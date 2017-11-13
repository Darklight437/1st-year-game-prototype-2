using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class GetAreaTest
* 
* this script was created to test the get area test
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
public class GetAreaTest : MonoBehaviour
{

    public Tiles tile;
    public int moveable;
    public List<Tiles> moveableTiles = new List<Tiles>();
    public Camera camera;


    /*
    * Update
    * void Update
    * 
    * this function runs every fram and is used to
    * see if the user has selected a tile or requested to get all tiles with in a area
    * 
    * @returns List<Tiles>
    */
    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            //sets out a raycast from the mouses position
            if (Physics.Raycast(ray, out hit))
            {
                //if we hit a tile we enter paint mode
                if (hit.transform.tag == "Tile")
                {
                    tile = hit.transform.GetComponent<Tiles>();
                }
            }
        }

        //gets the area
        if (Input.GetKeyDown("space"))
        {
            //reset our list of tilesand clear it then get the area
            foreach (Tiles tile in moveableTiles)
            {
                tile.tileType = eTileType.NORMAL;
                tile.GenerateRandomTileVariant();
            }

            moveableTiles.Clear();
            //moveableTiles = GetArea.GetAreaOfMoveable(tile, moveable);

            //redraw walkable area for easy visual checking
            foreach (Tiles tile in moveableTiles)
            {
                tile.tileType = eTileType.DEBUGGING;
                tile.GenerateRandomTileVariant();
            }

        }
	}
}

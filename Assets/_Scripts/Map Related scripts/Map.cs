using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class Map
* inherits MonoBehaviour
* 
* this class holds all the map tiles parented to its game object
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
public class Map : MonoBehaviour
{
    //list of all the tiles
    public List<Tiles> mapTiles;

    //maps width and height
    public int width;
    public int height;
    
	public virtual void SetUp(Statistics stats)
    {
		for (int i = 0; i < mapTiles.Count; i++) 
		{
			mapTiles [i].TileInit (this);	
		}

        SetTileEdges();

        for (int i = 0; i < mapTiles.Count; i++)
        {
            mapTiles[i].indexPos = i;
        }

    }
    
    /*
    * SetTileEdges
    * public void function
    * 
    * this function goes through all the tiles finding there
    * adjacents on North, South, East and west and connects them up for pathfinding
    * purposes
    * 
    * @returns nothing
    */
    private void SetTileEdges()
    {
        //goes through each of the nodes
        foreach (Tiles tileA in mapTiles)
        {
            tileA.tileEdges = new List<Tiles>();
            foreach (Tiles tileB in mapTiles)
            {
                //gets the offset of the x and z to determin if the tile is next to the one we are looking at
                float offsetX = tileA.transform.position.x - tileB.transform.position.x;
                float offsetZ = tileA.transform.position.z - tileB.transform.position.z;

                //true if tile to the East
                if (offsetX == 1 && offsetZ == 0)
                {
                    tileA.tileEdges.Add(tileB);
                }

                //true if tile to the South
                if (offsetX == 0 && offsetZ == -1)
                {
                    tileA.tileEdges.Add(tileB);
                }

                //true if tile to the West
                if (offsetX == -1 && offsetZ == 0)
                {
                    tileA.tileEdges.Add(tileB);
                }

                //true if tile to the North
                if (offsetX == 0 && offsetZ == 1)
                {
                    tileA.tileEdges.Add(tileB);
                }
            }
        }
    }

    /*
    * GetTileAtPos
    * public (Vector3 pos) function
    * 
    * this finds the tile at the passed int world location, it will round off the 
    * pos passed in x and z to the nearest int and set y to zero then search through 
    * the mapTiles in order to find the corrosponding tile in the world once it finds the tile it
    * returns it else it will retrun a null
    * 
    * @returns Tile
    */
    public Tiles GetTileAtPos(Vector3 pos)
    {
        Vector3 location = new Vector3(Mathf.RoundToInt(pos.x), 0, Mathf.RoundToInt(pos.z));

        foreach (Tiles tile in mapTiles)
        {
            if (tile.pos == location)
            {
                return tile;
            }
        }
        
        return null;
    }
}

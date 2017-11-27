using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChunk : MonoBehaviour
{
    public Tiles[] chunkTiles = new Tiles[5 * 5];

    //the cardial tile edge colour types
    public eChunkEdgeTypes north;
    public eChunkEdgeTypes east;
    public eChunkEdgeTypes south;
    public eChunkEdgeTypes west;

    public Tiles GetTileAtPos(Vector3 pos)
    {
        for (int i = 0; i < chunkTiles.Length; i++)
        {
            if (chunkTiles[i].pos == pos)
            {
                return chunkTiles[i];
            }
        }

        return null;
    }
	
}

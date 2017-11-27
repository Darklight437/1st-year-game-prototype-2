using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eChunkTypes
{
    NORMAL,
    DAMAGE,
    DEFENSE,
    IMPASSABLE,
    NON
}

public enum eChunkEdgeTypes
{
    WHITE,
    RED,
    GREEN,
    BLUE,
    NON
}

[System.Serializable]
public class Chunk : MonoBehaviour
{
    //the chunk type
    public eChunkTypes chunkType;

    //list of all the diffrent chunk types that will hold there varients
    public ChunkNormal chunkNormal;
    public ChunkDamage chunkDamage;
    public ChunkDefense chunkDefense;
    public ChunkImpassable chunkImpassable;

    //lists to hold the diffrent colours for the chunk edges
    public North northChunks;
    public East eastChunks;
    public South southChunks;
    public West westChunks;

    //actual chunk type varient that we are using
    private ChunkTypes m_usedChunkTypes;

    //satistics used for advanced random logic
    public Statistics statistics;

    //this gameobjects map chunk
    public MapChunk myChunk;

    //list of all adjacent chunks
    public Chunk northChunk;
    public Chunk eastChunk;
    public Chunk southChunk;
    public Chunk westChunk;


    /*
    * SetUsedChunkType
    * public void function
    * 
    * this function is used to determin what Chunk set should be assigned
    * to m_usedChunkTypes based on chunkType
    * 
    * @returns nothing
    */
    public void SetUsedChunkType()
    {
        switch (chunkType)
        {
            case eChunkTypes.NORMAL:
                m_usedChunkTypes = chunkNormal;
                return;

            case eChunkTypes.DAMAGE:
                m_usedChunkTypes = chunkDamage;
                return;

            case eChunkTypes.DEFENSE:
                m_usedChunkTypes = chunkDefense;
                return;

            case eChunkTypes.IMPASSABLE:
                m_usedChunkTypes = chunkImpassable;
                return;
            case eChunkTypes.NON:
                m_usedChunkTypes = null;
                return;

            default:
                return;
        }
    }

    /*
    * GenerateRandomTileVariant
    * public void function
    * 
    * this function will spawn in a varient of the tile type
    * stored in useTileSet
    * 
    * @returns nothing
    */
    public void GenerateRandomChunkVariant()
    {
        //make sure we are about to use the right chunk set
        SetUsedChunkType();

        if (m_usedChunkTypes == null)
        {
            return;
        }

        //if there is more then one type of tile in the set randomly pick on else just use the first one there
        //and make sure the tile is positioned in the right position
        if (m_usedChunkTypes.chunkTypes.Length > 1)
        {
            GameObject chunkSpawn = Instantiate(m_usedChunkTypes.chunkTypes[statistics.RandomChunkNum(chunkType)], new Vector3(0, 0, 0), Quaternion.identity);
            chunkSpawn.transform.SetParent(gameObject.transform);

            chunkSpawn.transform.localPosition = new Vector3(0, 0, 0);

            myChunk = chunkSpawn.GetComponent<MapChunk>();
        }
        else
        {
            GameObject chunkSpawn = Instantiate(m_usedChunkTypes.chunkTypes[0], new Vector3(0, 0, 0), Quaternion.identity);
            chunkSpawn.transform.SetParent(gameObject.transform);

            chunkSpawn.transform.localPosition = new Vector3(0, 0, 0);
            
            myChunk = chunkSpawn.GetComponent<MapChunk>();
        }
    }

    /*
    * GenerateRandomTileVariant
    * public void function
    * 
    * this function will spawn in a varient of the tile type
    * stored in useTileSet
    * 
    * @returns nothing
    */
    public void GenerateChunkVariant(GameObject mapChunk)
    {
        if (mapChunk == null)
        {
            Debug.LogError("EMPTY MAPCHUNK PASSES");
            return;
        }

        GameObject chunkSpawn = Instantiate(mapChunk, new Vector3(0, 0, 0), Quaternion.identity);
        chunkSpawn.transform.SetParent(gameObject.transform);

        chunkSpawn.transform.localPosition = new Vector3(0, 0, 0);

        myChunk = chunkSpawn.GetComponent<MapChunk>();
    }
}

[System.Serializable]
public class ChunkTypes
{
    public GameObject[] chunkTypes;
}

[System.Serializable]
public class ChunkNormal : ChunkTypes
{

}

[System.Serializable]
public class ChunkDamage : ChunkTypes
{

}

[System.Serializable]
public class ChunkDefense : ChunkTypes
{

}

[System.Serializable]
public class ChunkImpassable : ChunkTypes
{

}

[System.Serializable]
public class WangTiles
{
    public List<GameObject> white;
    public List<GameObject> red;
    public List<GameObject> green;
    public List<GameObject> blue;

}

[System.Serializable]
public class North : WangTiles
{

}

[System.Serializable]
public class East : WangTiles
{

}

[System.Serializable]
public class South : WangTiles
{

}

[System.Serializable]
public class West : WangTiles
{

}
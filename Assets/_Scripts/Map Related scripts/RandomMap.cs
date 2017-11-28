#if (UNITY_EDITOR) 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMap : Map
{
    public GameObject mapChunkPrefab;

    public List<Chunk> mapChunks = new List<Chunk>();

    public int scale;

    public float[] perlinNoise;

    public bool perlinNoiseBool;

    public bool wangTilesBool;
    private eChunkEdgeTypes m_north;
    private eChunkEdgeTypes m_east;
    private eChunkEdgeTypes m_south;
    private eChunkEdgeTypes m_west;
    private List<GameObject> m_toPick = new List<GameObject>();

    public override void SetUp(Statistics stats)
    {

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject holder = Instantiate(mapChunkPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                holder.transform.parent = transform;
                holder.transform.localPosition = new Vector3(x * 5, 0, z * 5);

                Chunk chunk = holder.GetComponent<Chunk>();

                chunk.myChunk = null;

                mapChunks.Add(chunk);
            }
        }

        if (perlinNoiseBool)
        {
            GenerateMapTilesPerlinNoise();
        }

        if (wangTilesBool)
        {
            WangTilesAlgorthim();
        }

        for (int i = 0; i < mapChunks.Count; i++)
        {
            for (int u = 0; u < mapChunks[i].myChunk.chunkTiles.Length; u++)
            {
                mapTiles.Add(mapChunks[i].myChunk.chunkTiles[u]);
            }
        }

        base.SetUp(stats);
    }

    /*
    * SetChunkEdges
    * public void function
    * 
    * this function goes through all the chunks finding there
    * adjacents on North, South, East and west and connects them up for map generation
    * purposes
    * 
    * @returns nothing
    */
    private void SetChunkEdges()
    {
        //goes through each of the nodes
        foreach (Chunk chunkA in mapChunks)
        {
            chunkA.eastChunk = null;
            chunkA.westChunk = null;
            chunkA.southChunk = null;
            chunkA.northChunk = null;

            foreach (Chunk chunkB in mapChunks)
            {
                //gets the offset of the x and z to determin if the tile is next to the one we are looking at
                float offsetX = chunkA.transform.position.x - chunkB.transform.position.x;
                float offsetZ = chunkA.transform.position.z - chunkB.transform.position.z;

                //true if tile to the East
                if (offsetX == 5 && offsetZ == 0)
                {
                    chunkA.eastChunk = chunkB;
                }

                //true if tile to the South
                if (offsetX == 0 && offsetZ == -5)
                {
                    chunkA.westChunk = chunkB;
                }

                //true if tile to the West
                if (offsetX == -5 && offsetZ == 0)
                {
                    chunkA.southChunk = chunkB;
                }

                //true if tile to the North
                if (offsetX == 0 && offsetZ == 5)
                {
                    chunkA.northChunk = chunkB;
                }
            }
        }
    }

    #region perlinNoise

    public void GenerateMapTilesPerlinNoise()
    {
        CalcPerlInNoiseGrid();

        int num = 0;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                mapChunks[num].chunkType = ReturnChunkType(x, z);
                mapChunks[num].GenerateRandomChunkVariant();

                num++;
            }
        }
    }

    public void CalcPerlInNoiseGrid()
    {
        perlinNoise = new float[width * height];

        float seed = Time.realtimeSinceStartup;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                perlinNoise[y * width + x] = RandomNum(x, y, seed);
            }
        }
    }

    public float RandomNum(float x, float y, float seed)
    {
        float xCord = (float)x / width * scale;
        float yCord = (float)y / height * scale;

        float holder = Mathf.PerlinNoise(xCord + seed, yCord + seed);

        return holder;
    }


    public eChunkTypes ReturnChunkType(int x, int y)
    {
        float num = perlinNoise[y * width + x];

        if (num < 0.41f)
        {
            return eChunkTypes.IMPASSABLE;
        }

        if (num < 0.61f)
        {
            return eChunkTypes.NORMAL;
        }

        if (num < 0.71f)
        {
            return eChunkTypes.DEFENSE;
        }

        if (num < 1.01f)
        {
            return eChunkTypes.DAMAGE;
        }

        return eChunkTypes.NORMAL;
    }
    #endregion

    #region WangTiles

    public void WangTilesAlgorthim()
    {
        for (int i = 0; i < mapChunks.Count; i++)
        {
            m_north = eChunkEdgeTypes.NON;
            m_east = eChunkEdgeTypes.NON;
            m_south = eChunkEdgeTypes.NON;
            m_west = eChunkEdgeTypes.NON;

            if (mapChunks[i].northChunk != null)
            {
                if (mapChunks[i].northChunk.myChunk != null)
                {
                    m_north = mapChunks[i].northChunk.myChunk.south;
                }
            }

            if (mapChunks[i].eastChunk != null)
            {
                if (mapChunks[i].eastChunk.myChunk != null)
                {
                    m_north = mapChunks[i].eastChunk.myChunk.west;
                }
            }

            if (mapChunks[i].southChunk != null)
            {
                if (mapChunks[i].southChunk.myChunk != null)
                {
                    m_north = mapChunks[i].southChunk.myChunk.north;
                }
            }

            if (mapChunks[i].westChunk != null)
            {
                if (mapChunks[i].westChunk.myChunk != null)
                {
                    m_north = mapChunks[i].westChunk.myChunk.east;
                }
            }

            ToPickSetUP(m_north);
            ToPickSetUP(m_east);
            ToPickSetUP(m_south);
            ToPickSetUP(m_west);

        }
    }

    public void ToPickSetUP(eChunkEdgeTypes type)
    {
        switch (type)
        {
            case eChunkEdgeTypes.WHITE:
                foreach (GameObject chunk in mapChunks[0].northChunks.white)
                {
                    m_toPick.Add(chunk);
                }
                return;

            case eChunkEdgeTypes.RED:
                foreach (GameObject chunk in mapChunks[0].northChunks.red)
                {
                    m_toPick.Add(chunk);
                }
                return;

            case eChunkEdgeTypes.GREEN:
                foreach (GameObject chunk in mapChunks[0].northChunks.green)
                {
                    m_toPick.Add(chunk);
                }
                return;

            case eChunkEdgeTypes.BLUE:
                foreach (GameObject chunk in mapChunks[0].northChunks.blue)
                {
                    m_toPick.Add(chunk);
                }
                return;
        }

        Debug.LogError("INVALID CHUNK TYPE PASSED");
        return;
    }

    #endregion


}

#endif

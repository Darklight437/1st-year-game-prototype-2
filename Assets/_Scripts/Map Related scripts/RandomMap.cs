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

    public bool diamondSquareAlgorithm;
    public int size;
    public int divisions;
    public float worldHeight;
    public eChunkTypes[] chunkLayout;
    public int recusionAmount;

    public override void SetUp()
    {

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject holder = Instantiate(mapChunkPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                holder.transform.parent = transform;
                holder.transform.localPosition = new Vector3(x * 5, 0, z * 5);

                Chunk chunk = holder.GetComponent<Chunk>();
                
                mapChunks.Add(chunk);
            }
        }

        if (perlinNoiseBool)
        {
            GenerateMapTilesPerlinNoise();
        }

        if (diamondSquareAlgorithm)
        {
            GenerateDiamondSquareAlgorithm();
        }
        
        for (int i = 0; i < mapChunks.Count; i++)
        {
            for (int u = 0; u < mapChunks[i].myChunk.chunkTiles.Length; u++)
            {
                mapTiles.Add(mapChunks[i].myChunk.chunkTiles[u]);
            }
        }

        base.SetUp();
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
                mapChunks[num].chunkType = ReturnChunkType(x,z);
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

    #region DiamondSquare

    public void GenerateDiamondSquareAlgorithm()
    {
        recusionAmount = (divisions + 1) * (divisions + 1);
        chunkLayout = new eChunkTypes[recusionAmount];

        float halfSize = size * 0.5f;
        float divisionSize = size / divisions;

        for (int i = 0; i <= divisions; i++)
        {
            for (int u = 0; u <= divisions; u++)
            {
                chunkLayout[i * (divisions + 1) + u] = eChunkTypes.NORMAL;
            }
        }

        for (int i = 0; i < mapChunks.Count; i++)
        {
            mapChunks[i].chunkType = chunkLayout[i];
            mapChunks[i].GenerateRandomChunkVariant();
        }
    }

#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPreveiw : MonoBehaviour
{

    public Map map;
    public GameObject mapPreview;

    int mapWidth = 0;
    int mapHeight = 0;
    int mapOffsetX = 0;
    int mapOffsetY = 0;

    Texture2D texture;

    private void Awake()
    {
        mapWidth = map.width;
        mapHeight = map.height;

        mapOffsetX = 0;
        mapOffsetY = 0;

        if (map == null)
        {

        }

        texture = new Texture2D(0, 0);
        
        if (mapWidth > mapHeight)
        {
            mapOffsetY = (int)((mapWidth - mapHeight) * 0.5f);
        }
        else if(mapWidth < mapHeight)
        {
            mapOffsetX = (int)((mapHeight - mapWidth) * 0.5f);
        }

        if (mapWidth > mapHeight)
        {
            if (mapHeight + (mapOffsetY * 2) != mapWidth)
            {
                texture.Resize(mapWidth, mapHeight + (mapOffsetY * 2) + 1);
            }
            texture.Resize(mapWidth, mapHeight + (mapOffsetY * 2));
        }
        else if (mapWidth < mapHeight)
        {
            if (mapWidth + (mapOffsetX * 2) != mapHeight)
            {
                texture.Resize(mapWidth + (mapOffsetX * 2) + 1, mapHeight);
            }
            texture.Resize(mapWidth + (mapOffsetX * 2), mapHeight);
        }
        else if (mapWidth == mapHeight)
        {
            texture.Resize(mapWidth, mapHeight);
        }
        else
        {
            Debug.LogError("WHAT THE FUCK??");
        }

        MakeTexture();

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        texture.Apply();

        mapPreview.GetComponent<Renderer>().material.mainTexture = texture;

    }

    public void MakeTexture()
    {
        if (mapWidth > mapHeight)
        {
            SetTexture(mapWidth);
        }
        else if (mapWidth < mapHeight)
        {
            SetTexture(mapHeight);
        }
        else
        {
            SetTexture(mapHeight);
        }

        for (int i = 0; i < map.mapTiles.Count; i++)
        {
            texture.SetPixel((int)(map.mapTiles[i].transform.position.x + mapOffsetY ), (int)(map.mapTiles[i].transform.position.z + mapOffsetX), GetColor(map.mapTiles[i].tileType));
        }

        texture.Apply();

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Debug.Log(texture.GetPixel(x,y));
            }
        }
    }

    private void SetTexture(int num)
    {
        Color color = new Color(0, 0, 0);

        for (int x = 0; x < num; x++)
        {
            for (int y = 0; y < num; y++)
            {
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
    }

    private Color GetColor(eTileType type)
    {
        switch (type)
        {
            case eTileType.NORMAL:
                return new Color(0.75f, 0.6f, 0.5f);

            case eTileType.DAMAGE:
                return new Color(0, 1, 0);

            case eTileType.DEFENSE:
                return new Color(0.65f, 0.16f, 0.16f);

            case eTileType.IMPASSABLE:
                return new Color(0.25f, 0.25f, 0.25f);

            case eTileType.NULL:
                return new Color(0, 0, 0);

            case eTileType.DEBUGGING:
                return new Color(0, 0, 0);

            case eTileType.PLACABLEDEFENSE:
                return new Color(0.65f, 0.16f, 0.16f);

            case eTileType.PLACABLETRAP:
                return new Color(0, 1, 0);
                
        }

        Debug.LogError("NOT VALID TYPE PASSED");
        return new Color(0, 0, 0);
    }
}

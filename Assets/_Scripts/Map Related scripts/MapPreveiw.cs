using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MapPreveiw : MonoBehaviour
{

    public Map map;
    public GameObject mapPreview;

    int mapWidth = 0;
    int mapHeight = 0;
    int mapOffsetX = 0;
    int mapOffsetY = 0;

    private Texture2D m_texture;

    private string m_filePath;

    string[] holder;
    private List<string> m_maps = new List<string>();

    private void Awake()
    {
        mapOffsetX = 0;
        mapOffsetY = 0;
        
        if (map == null)
        {
            m_filePath = Application.dataPath + "/Resources/";
            holder = Directory.GetFiles(m_filePath + "maps/", "*.prefab");

            for (int i = 0; i < holder.Length; i++)
            {
                m_maps.Add(holder[i]);
            }

            ////////////////////////////////////////////////////////////

            GameObject test = new GameObject();

            for (int i = 0; i < m_maps.Count; i++)
            {
                test = Resources.Load(m_maps[i], typeof(GameObject)) as GameObject;
            }

            //GameObject test2 = test as GameObject;

            map = test.GetComponent<Map>();

            ////////////////////////////////////////////////////////////

        }

        mapWidth = map.width;
        mapHeight = map.height;

        m_texture = new Texture2D(0, 0);
        
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
                m_texture.Resize(mapWidth, mapHeight + (mapOffsetY * 2) + 1);
            }
            m_texture.Resize(mapWidth, mapHeight + (mapOffsetY * 2));
        }
        else if (mapWidth < mapHeight)
        {
            if (mapWidth + (mapOffsetX * 2) != mapHeight)
            {
                m_texture.Resize(mapWidth + (mapOffsetX * 2) + 1, mapHeight);
            }
            m_texture.Resize(mapWidth + (mapOffsetX * 2), mapHeight);
        }
        else if (mapWidth == mapHeight)
        {
            m_texture.Resize(mapWidth, mapHeight);
        }
        else
        {
            Debug.LogError("WHAT THE FUCK??");
        }

        MakeTexture();

        m_texture.filterMode = FilterMode.Point;
        m_texture.wrapMode = TextureWrapMode.Clamp;

        m_texture.Apply();

        mapPreview.GetComponent<Image>().material.mainTexture = m_texture;

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
            m_texture.SetPixel((int)(map.mapTiles[i].transform.position.x + mapOffsetY ), (int)(map.mapTiles[i].transform.position.z + mapOffsetX), GetColor(map.mapTiles[i].tileType));
        }

        m_texture.Apply();
    }

    private void SetTexture(int num)
    {
        Color color = new Color(0, 0, 0);

        for (int x = 0; x < num; x++)
        {
            for (int y = 0; y < num; y++)
            {
                m_texture.SetPixel(x, y, color);
            }
        }

        m_texture.Apply();
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

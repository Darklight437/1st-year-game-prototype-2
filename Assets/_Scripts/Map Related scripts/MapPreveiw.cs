using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

/*
* class MapPreveiw
* inherits MonoBehaviour
* 
* this class reads in a map and creates a texture to show it off in the menu scene
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
public class MapPreveiw : MonoBehaviour
{
    //refrence to the map we plan to make a texture off
    public Map map;
    //refrence to the game object we will be setting the texture to
    public GameObject mapPreview;

    //the map width
    int mapWidth = 0;
    //map height
    int mapHeight = 0;
    //this is used for maps with a greater Y then X
    int mapOffsetX = 0;
    //this is used for maps with a greater X then Y
    int mapOffsetY = 0;

    //the texture we have map from the map
    private Texture2D m_texture;
    
    private List<string> m_maps = new List<string>();

    private void Awake()
    {
        //set default values
        mapOffsetX = 0;
        mapOffsetY = 0;
        
        mapWidth = map.width;
        mapHeight = map.height;

        m_texture = new Texture2D(0, 0);
        
        //creat the offsets if needed
        if (mapWidth > mapHeight)
        {
            mapOffsetY = (int)((mapWidth - mapHeight) * 0.5f);
        }
        else if(mapWidth < mapHeight)
        {
            mapOffsetX = (int)((mapHeight - mapWidth) * 0.5f);
        }

        //appropriatly resize the texture with our map size allways making sure we have a square texture
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
            //we should never reach this point
            Debug.LogError("WHAT THE FUCK??");
        }

        //function used to make the texture
        MakeTexture();

        //SetTexture some texture values so it looks good when applied
        m_texture.filterMode = FilterMode.Point;
        m_texture.wrapMode = TextureWrapMode.Clamp;

        //make sure we actually apply the changes to the texture
        m_texture.Apply();

        //apply the texture we have made to our game object
        mapPreview.GetComponent<Image>().material.mainTexture = m_texture;

    }

    /*
    * MakeTexture
    * public void function
    * 
    * this paints in our map preview with our map making
    * our tecture to display
    * 
    * @returns nothing
    */
    public void MakeTexture()
    {
        //set texture to be black so we can start painting over it
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

        //goes through all of our tiles in the map and paints our texture with them
        for (int i = 0; i < map.mapTiles.Count; i++)
        {
            //add offset to tile positions, this is only really need for rectangular maps so that they still fit into a square texture neatly and do not streachcall function to get tiles
            //colour based on the tile type
            m_texture.SetPixel((int)(map.mapTiles[i].transform.position.x + mapOffsetY ), (int)(map.mapTiles[i].transform.position.z + mapOffsetX), GetColor(map.mapTiles[i].tileType));
        }

        //apply texture changes
        m_texture.Apply();
    }

    /*
    * SetTexture
    * public void function
    * 
    * sets the tecture to be qual to plack before we paint on our map
    * 
    * 
    * @param int num - the amount we want to loop through painting them texture black
    * @returns nothing
    */
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

        //apply texture changes
        m_texture.Apply();
    }

    /*
    * SetTexture
    * public void function
    * 
    * this function takes in a tile type thenpasses that into a switch statmeant,
    * depending on the tile type passed in it will pas out an apporpriate color
    * 
    * @param eTileType type - the current tile type we wish to paint in to our map preview texture
    * @returns Color - used to paint in our map preview texture
    */
    private Color GetColor(eTileType type)
    {
        switch (type)
        {
            case eTileType.NORMAL:
                return new Color(0.75f, 0.6f, 0.45f);

            case eTileType.DAMAGE:
                return new Color(0, 1, 0);

            case eTileType.DEFENSE:
                return new Color(0.53f, 0.36f, 0.12f);

            case eTileType.IMPASSABLE:
                return new Color(0.25f, 0.25f, 0.25f);

            case eTileType.NULL:
                return new Color(0, 0, 0);

            case eTileType.DEBUGGING:
                return new Color(0, 0, 0);

            case eTileType.PLACABLEDEFENSE:
                return new Color(0.53f, 0.36f, 0.12f);

            case eTileType.PLACABLETRAP:
                return new Color(0, 1, 0);
        }

        //tile type we passed in was not valid
        Debug.LogError("NOT VALID TYPE PASSED");
        return new Color(0, 0, 0);
    }
}

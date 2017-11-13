#if (UNITY_EDITOR) 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
* class MapEditorScript
* inherits MonoBehaviour
* 
* this class is the script that goes on to a gameobject in our scene so that
* we can acces the custom buttons in MapEditor and holds all the functions 
* for modifing the map here
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
public class MapEditorScript : MonoBehaviour
{
    //the tile we wish to paint to a map
    private eTileType m_paintType = eTileType.NORMAL;

    //the tile prefab that we use to make the map up of
    public GameObject tileObj;

    //the maps length and width of tiles that we want to generate
    public uint mapLength;
    public uint mapWidth;

    //a refrence to the active map we are editing, used for paint all and update tile functions
    public GameObject world;

    //boolean for weather or not paint mode is on or off
    public bool enablePaint = false;

    /*
    * GenerateBasMap
    * public void function
    * 
    * this function is used to create our blank map of tiles
    * to be painted over and a gameobject to parent them to
    * 
    * @returns nothing
    */
    public void GenerateBasMap()
    {
        //new world obj
        world = new GameObject();

        //sets up the world obj
        world.name = "World GameObject";
        world.tag = "World";
        Map map = world.AddComponent<Map>();
        map.mapTiles = new List<Tiles>();
        map.width = (int)mapWidth;
        map.height = (int)mapLength;

        //nestd for loop to instantiate the map
        for (int x = 0; x < mapLength; x++)
        {
            for (int z = 0; z < mapWidth; z++)
            {
                GameObject mapTile = Instantiate(tileObj, new Vector3(x, 0, z), Quaternion.identity);

                //set parent of tile to the world obj and there tile type to a bland one
                //then spawns the moddle in
                mapTile.transform.SetParent(world.transform);
                mapTile.GetComponent<Tiles>().tileType = eTileType.NORMAL;
                mapTile.GetComponent<Tiles>().GenerateBaseTile();

                //adds the tile to the worlds map script of tiles
                map.mapTiles.Add(mapTile.GetComponent<Tiles>());
            }
        }
    }

    /*
    * TogglePaint
    * public void function
    * 
    * this function toggles on and off paint mode
    * 
    * @returns nothing
    */
    public void TogglePaint()
    {
        if (enablePaint == false)
        {
            enablePaint = true;
        }
        else
        {
            enablePaint = false;
        }
    }

    /*
    * SetPaintTypeNormalTile
    * public void function
    * 
    * this function sets paint mode to normal tile
    * 
    * @returns nothing
    */
    public void SetPaintTypeNormalTile()
    {
        m_paintType = eTileType.NORMAL;
    }

    /*
    * SetPaintTypeDamageTile
    * public void function
    * 
    * this function sets paint mode to Damage tile
    * 
    * @returns nothing
    */
    public void SetPaintTypeDamageTile()
    {
        m_paintType = eTileType.DAMAGE;
    }

    /*
    * SetPaintTypeDefenseTile
    * public void function
    * 
    * this function sets paint mode to Defense tile
    * 
    * @returns nothing
    */
    public void SetPaintTypeDefenseTile()
    {
        m_paintType = eTileType.DEFENSE;
    }

    /*
    * SetPaintTypeImpassibleTile
    * public void function
    * 
    * this function sets paint mode to Impassable tile
    * 
    * @returns nothing
    */
    public void SetPaintTypeImpassibleTile()
    {
        m_paintType = eTileType.IMPASSABLE;
    }

    /*
    * PaintAll
    * public void function
    * 
    * this function paints allthe child tiles in the
    * world refrense to what ever tile type is selected
    * 
    * @returns nothing
    */
    public void PaintAll()
    {
        //goes through each tile sets there tile type and tells them
        //to redraw there tile to reflect that
        foreach (Transform child in world.transform)
        {
            child.GetComponent<Tiles>().tileType = m_paintType;
            child.GetComponent<Tiles>().GenerateBaseTile();
        }
    }

    /*
    * Paint
    * public void function
    * 
    * this function handles the painting of tiles on the map
    * it does this through ray casts
    * 
    * @returns nothing
    */
    public void Paint()
    {
        RaycastHit hit;
        Ray  ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        
        //sets out a raycast from the mouses position
        if (Physics.Raycast(ray, out hit))
        {
            //if we hit a tile we enter paint mode
            if (hit.transform.tag == "Tile")
            {
                //get the tile script off the object
                Tiles tile = hit.transform.GetComponent<Tiles>();

                //get its tile type
                eTileType checkType = tile.tileType;

                //if we right click we just want to iterrate through the tile types
                if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
                {
                    //if the tile type is at the then of the enum we llop back to the begining 
                    if ((tile.tileType + 1) == eTileType.NULL)
                    {
                        tile.tileType = eTileType.NORMAL;
                        tile.GenerateBaseTile();

                        return;
                    }
                    
                    //assuming that we are not at the end we change the tile to the next type
                    //add tell it to re generate the tile it is displaying
                    tile.tileType++;
                    tile.GenerateBaseTile();
                }
                else if (checkType != m_paintType)
                {
                    //if we are not trying to change the tile to the same type it is then we set
                    //its type to the one selected by our paint mode and tell it to re generate that tile
                    tile.tileType = m_paintType;
                    tile.GenerateBaseTile();
                }
            }
        }
    }

    /*
    * UpdateTilePrefabs
    * public void function
    * 
    * this function goes through our world and gathers up all of its old tile prefabs
    * it then goes through them all copying them with the latest uptodate prefabs and
    * replaces them so old maps can be updated with the new tiles
    * 
    * @returns nothing
    */
    public void UpdateTilePrefabs()
    {
        List<Tiles> mapTiles = world.GetComponent<Map>().mapTiles;

        List<Tiles> newTiles = new List<Tiles>();

        foreach (Tiles tile in mapTiles)
        {
            GameObject mapTile = Instantiate(tileObj, tile.transform.position, Quaternion.identity);

            mapTile.GetComponent<Tiles>().tileType = tile.tileType;
            mapTile.GetComponent<Tiles>().GenerateBaseTile();

            mapTile.transform.SetParent(world.transform);

            newTiles.Add(mapTile.GetComponent<Tiles>());

            DestroyImmediate(tile.gameObject);
        }

        world.GetComponent<Map>().mapTiles = newTiles;
    }

    /*
    * GenerateMapChunk
    * public void function
    * 
    * this funcktion generates a simple 5,5 map chunk
    * 
    * @returns nothing
    */
    public void GenerateMapChunk()
    {
        //new world obj
        world = new GameObject();

        //sets up the chunk obj
        world.name = "Map Chunk";
        world.tag = "MapChunk";
        MapChunk map = world.AddComponent<MapChunk>();

        int num = 0;

        //nestd for loop to instantiate the map
        for (int x = 0; x < 5; x++)
        {
            for (int z = 0; z < 5; z++)
            {
                GameObject mapTile = Instantiate(tileObj, new Vector3(x, 0, z), Quaternion.identity);

                //set parent of tile to the world obj and there tile type to a bland one
                //then spawns the moddle in
                mapTile.transform.SetParent(world.transform);
                mapTile.GetComponent<Tiles>().tileType = eTileType.NORMAL;
                mapTile.GetComponent<Tiles>().GenerateBaseTile();

                //adds the tile to the map chunk script of tiles
                map.chunkTiles[num] = mapTile.GetComponent<Tiles>();

                num++;
            }
        }
    }
}
#endif
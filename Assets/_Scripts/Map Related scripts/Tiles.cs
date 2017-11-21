using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* eTileType
* public enum
* 
* this enum holds all the diffrent tile types
* 
*/
[System.Serializable]
public enum eTileType
{
    NORMAL,
    DAMAGE,
    DEFENSE,
    IMPASSABLE,
    NULL,
    DEBUGGING,
    PLACABLEDEFENSE,
    PLACABLETRAP
}

/*
* class Tiles
* inherits MonoBehaviour
* 
* this class holds all the tile Info and refrences to all the diffrent tile types
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
[System.Serializable]
public class Tiles : MonoBehaviour
{
    //this dictates what tile type this tile is
    public eTileType tileType;
    
    //this is the tiles that are next to it and is connected to
    //for pathfinding purposes
    public List<Tiles> tileEdges;

    //these are the classes that hold the diffrent tile varients inside of them
    //so that when we play the game we have more variaty
    public NormalTile normalTile;
    public DamageTile damageTile;
    public DefenseTile defenseTile;
    public ImpassableTile impassableTile;
    public PlacableDefenceTile placableDefenceTile;
    public PlacableTrapTile placableTrapTile;

    //these tile set is used for debbuging purposes
    public DebuggingTile debuggingTile;

    //this is the set of tile varients we actually want to usee when the game is running
    //this should make it very easy to change on the fly
    public TileTypes useTileSet;

    //the prefab we will spawn to show what tiles are walkable
    public GameObject walkablePrefab;
    //the gameobject that is used to show walkable that we can toggle on and off
    public GameObject walkableHighLight;

    //prefabe used to show your units attack range
    public GameObject attackRangePrefab;
    //the refrence the the gameobject to show the units attack range
    public GameObject attackRangeHighLight;
    
    //the prefab to show the area an enemy could attack you on there turn
    public GameObject dangerZoneRangePrefab;
    //the variable that holds the tile modifier that shows you where you can be attacked
    public GameObject dangerZoneRangeHighLight;

    //the statistics used for random number logic
	public Statistics statistics;

    //unit that is on the tile
    public Unit unit = null;

    //check if a tile is healing
    private bool m_isHealing = false;
    private int m_playerID = -1;

    private float originalY = 0.0f;
    private float originalHeight = 0.0f;

    private float elevatedHeight = 0.33f;

    //this is the position in the list on the map script where this tile is
    public int indexPos;

    //refrence to the map the tile is parented to
    private Map m_myMap;

    //this set the render ques of tile objects
    public int renderQueTile;

    public MediPack tileMediPack;

    //tile values for pathfinding purposes 
    private float m_gcost;
    public float GCost
    {
        get
        {
            return m_gcost;
        }
        set
        {
            m_gcost = value;
        }
    }

    private float m_hcost;
    public float HCost
    {
        get
        {
            return m_hcost;
        }
        set
        {
            m_hcost = value;
        }
    }

    public float FCost
    {
        get
        {
            return (m_gcost + m_hcost);
        }
    }

    //determins if this tile is passible
    public bool IsPassible(Unit aUnit)
    {
        if (unit != null && aUnit.playerID != unit.playerID)
        {
            return false;
        }

        switch (tileType)
        {
            case eTileType.NORMAL:
                return true;
            case eTileType.DAMAGE:
                return true;
            case eTileType.DEFENSE:
                return true;
            case eTileType.IMPASSABLE:
                return false;
            case eTileType.NULL:
                return false;
            case eTileType.DEBUGGING:
                return true;
            case eTileType.PLACABLEDEFENSE:
                return true;
            case eTileType.PLACABLETRAP:
                return true;
        }

        return false;
    }

    public bool IsPassible()
    {
        switch (tileType)
        {
            case eTileType.NORMAL:
                return true;
            case eTileType.DAMAGE:
                return true;
            case eTileType.DEFENSE:
                return true;
            case eTileType.IMPASSABLE:
                return false;
            case eTileType.NULL:
                return false;
            case eTileType.DEBUGGING:
                return true;
            case eTileType.PLACABLEDEFENSE:
                return true;
            case eTileType.PLACABLETRAP:
                return true;
        }

        return false;
    }

    //determines if this tile has healing effects on it
    public bool IsHealing(bool value, Unit unit)
    {
        //reset the healing on the tile
        if (m_isHealing && value != true)
        {
            m_isHealing = false;

            Destroy(tileMediPack.currMedPack);
            Destroy(tileMediPack.usedHealthMist);
            Destroy(tileMediPack.usedTeamParticals);

            if (m_playerID != unit.playerID)
            {
                Destroy(Instantiate(tileMediPack.destroyedParticals, 
                                    new Vector3(transform.position.x, 
                                    0.3f, 
                                    transform.position.z), 
                                    tileMediPack.destroyedParticals.transform.rotation), 
                                    5);

                return false;
            }

            Destroy(Instantiate(tileMediPack.usedParticals, 
                                new Vector3(transform.position.x,
                                0.3f,
                                transform.position.z),
                                tileMediPack.destroyedParticals.transform.rotation),
                                5);

            return true;
        }

        if (value)
        {
            m_isHealing = value;

            if (m_isHealing)
            {
                m_playerID = unit.playerID;

                if (m_playerID == 1)
                {
                    tileMediPack.currMedPack = Instantiate(tileMediPack.redTeamMediPack, transform.position, Quaternion.identity);
                    tileMediPack.usedHealthMist = Instantiate(tileMediPack.redTeamHealthMist, new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.identity);
                    tileMediPack.usedTeamParticals = Instantiate(tileMediPack.redTeamParticals, new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.identity);
                }

                if (m_playerID == 0)
                {

                }
            }
        }


        return false;
    }

    //this is the tiles parent for pathfinding purposes
    public Tiles parent;
    
    //the tiles world position for debugging purposes
    public Vector3 pos;

    /*
    * Start
    * public void function
    * 
    * this function is called at the start of the scripts life
    * in play mode
    * 
    * @returns nothing
    */
    private void Start()
    {
		
    }

	public void TileInit(Map map)
	{
        m_myMap = map;

		GenerateRandomTileVariant();
		pos = gameObject.transform.position;

		GenerateTileModifiers();

		originalY = pos.y;
		originalHeight = GetComponent<BoxCollider>().size.y;
	}

    /*
    * Update 
    *  
    * gets called once per frame
    * 
    * @returns void
    */
    private void Update()
    {
        BoxCollider box = GetComponent<BoxCollider>();

        if (unit == null)
        {
            box.center = new Vector3(0.0f, originalY, 0.0f);
            box.size = new Vector3(box.size.x, originalHeight, box.size.z);
        }
        else
        {
            box.center = new Vector3(0.0f, elevatedHeight * 0.5f + originalY, 0.0f);
            box.size = new Vector3(box.size.x, elevatedHeight, box.size.z);
        }
    }

    /*
    * GenerateTileModifiers
    * public void function
    * 
    * this function generates the tile modifiers to show
    * unit movmeant range and attack range
    * 
    * @returns nothing
    */
    public void GenerateTileModifiers()
    {
        GameObject obj = Instantiate(walkablePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        obj.transform.SetParent(gameObject.transform);
        walkableHighLight = obj;
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.gameObject.SetActive(false);

        GameObject obj2 = Instantiate(attackRangePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        obj2.transform.SetParent(gameObject.transform);
        attackRangeHighLight = obj2;
        obj2.transform.localPosition = new Vector3(0, 0, 0);
        obj2.gameObject.SetActive(false);

        GameObject obj3 = Instantiate(dangerZoneRangePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        obj3.transform.SetParent(gameObject.transform);
        dangerZoneRangeHighLight = obj3;
        obj3.transform.localPosition = new Vector3(0, 0, 0);
        obj3.gameObject.SetActive(false);
    }

    /*
    * SetUsedTileType
    * public void function
    * 
    * this function is used to determin what tile set should be assigned
    * to useTileSet based on tileType
    * 
    * @returns nothing
    */
    public void SetUsedTileType()
    {
        switch (tileType)
        {
            case eTileType.NORMAL:
                useTileSet = normalTile;
                break;

            case eTileType.DAMAGE:
                useTileSet = damageTile;
                break;

            case eTileType.DEFENSE:
                useTileSet = defenseTile;
                break;

            case eTileType.IMPASSABLE:
                useTileSet = impassableTile;
                break;

            case eTileType.DEBUGGING:
                useTileSet = debuggingTile;
                break;

            case eTileType.PLACABLEDEFENSE:
                useTileSet = placableDefenceTile;
                break;

            case eTileType.PLACABLETRAP:
                useTileSet = placableTrapTile;
                break;
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
    public void GenerateRandomTileVariant()
    {
        //first delete our current child
        foreach (Transform child in transform)
        {
            if (child.tag != "TileModifier")
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        //make sure we are about to use the right tile set
        SetUsedTileType();

        //if there is more then one type of tile in the set randomly pick on else just use the first one there
        //and make sure the tile is positioned in the right position
        if (useTileSet.tileTypes.Length > 1)
        {
            GameObject tileSpawn = Instantiate(useTileSet.tileTypes[statistics.RandomTileNum(tileType)], new Vector3(0, 0, 0), Quaternion.identity);
            tileSpawn.transform.SetParent(gameObject.transform);

            tileSpawn.transform.localPosition = new Vector3(0, 0, 0);

            foreach (Transform trans in tileSpawn.GetComponentsInChildren<Transform>())
            {
                if (trans.tag == "Renderer Que")
                {
                    trans.GetComponent<Renderer>().material.renderQueue = renderQueTile;
                    Debug.Log(trans.GetComponent<Renderer>().material.renderQueue);
                }
            }
        }
        else
        {
            GameObject tileSpawn = Instantiate(useTileSet.tileTypes[0], new Vector3(0, 0, 0), Quaternion.identity);
            tileSpawn.transform.SetParent(gameObject.transform);

            tileSpawn.transform.localPosition = new Vector3(0, 0, 0);

            foreach (Transform trans in tileSpawn.transform)
            {
                if (trans.tag == "Renderer Que")
                {
                    trans.GetComponent<Renderer>().material.renderQueue = renderQueTile;
                    Debug.Log(trans.GetComponent<Renderer>().material.renderQueue);
                }
            }
        }

    }

    /*
    * GenerateBaseTile
    * public void function
    * 
    * this function will spawn in the base varient of a tile
    * this is used mainly for the map editor as it needs a special type 
    * of delete obj and we only use the base varient of a tile for simplicitys sake
    * 
    * @returns nothing
    */
    public void GenerateBaseTile()
    {
        //delets the old tile that was showing
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        //make sure we are using the right tile set
        SetUsedTileType();

        //spawns in the base tile varient type and sets up its parent and position correctly
        GameObject tileSpawn = Instantiate(useTileSet.tileTypes[0], new Vector3(0, 0, 0), Quaternion.identity);
        tileSpawn.transform.SetParent(gameObject.transform);

        tileSpawn.transform.localPosition = new Vector3(0, 0, 0);
    }
}

/*
* class MediPack
* 
* this holds all the prefabs for medipack related stuff
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
[System.Serializable]
public class MediPack
{
    //OH BOY IT IS MEDICPACK STUFF
    public GameObject redTeamMediPack;
    public GameObject blueTeamMediPack;

    public GameObject currMedPack;

    public GameObject redTeamHealthMist;
    public GameObject redTeamParticals;

    public GameObject blueTeamHealthMist;
    public GameObject blueTeamParticals;

    public GameObject usedHealthMist;
    public GameObject usedTeamParticals;

    public GameObject usedParticals;
    public GameObject destroyedParticals;
}

/*
* class TileTypes
* 
* this class is used to link all tile types up 
* for visual purposes so that we can use useTileSet from tiles easily and with out problems
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
[System.Serializable]
public class TileTypes
{
    //this will hold all the tile varient types for all the diffrent tile types that inherit from this
    public GameObject[] tileTypes;
}

/*
* class Tiles
* inherits TileTypes
* 
* this class holds all the varient types for normal tiles
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
[System.Serializable]
public class NormalTile : TileTypes
{
}

/*
* class DamageTile
* inherits TileTypes
* 
* this class holds all the varient types for Damage tiles
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
[System.Serializable]
public class DamageTile : TileTypes
{
}

/*
* class DefenseTile
* inherits TileTypes
* 
* this class holds all the varient types for Defense tiles
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
[System.Serializable]
public class DefenseTile : TileTypes
{
}

/*
* class ImpassableTile
* inherits TileTypes
* 
* this class holds all the varient types for Impassable tiles
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
[System.Serializable]
public class ImpassableTile : TileTypes
{
}

/*
* class DebuggingTile
* inherits TileTypes
* 
* this class tile set is used for debugging purposes
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
[System.Serializable]
public class DebuggingTile : TileTypes
{
}

/*
* class PlacableDefenceTile
* inherits TileTypes
* 
* this class tile set is used for debugging purposes
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
[System.Serializable]
public class PlacableDefenceTile : TileTypes
{
}

/*
* class PlacableTrapTile
* inherits TileTypes
* 
* this class tile set is used for debugging purposes
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
[System.Serializable]
public class PlacableTrapTile : TileTypes
{
}
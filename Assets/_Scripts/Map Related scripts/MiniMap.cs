using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
* class MiniMap
* inherits MonoBehaviour
* 
* this class is used to read in current infomation
* on the map related to the active player and create a texture
* to acuratly display that infomation
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
public class MiniMap : MonoBehaviour
{
    //refrence to the game map
    public Map map;
    
    //the object we will be sticking our mini map texture on to
    public RawImage miniMap;

    //the gameManager
    public GameManagment gameManagment;
    
    //info for the map and any offsets needed
    int mapWidth = 0;
    int mapHeight = 0;
    int mapOffsetX = 0;
    int mapOffsetY = 0;

    //link to the current active player
    private BasePlayer m_activeplayer;

    //the mini map texture we will be making
    private Texture2D m_texture;

    //list of all tiles the player can see
    private List<Tiles> m_sightTiles = new List<Tiles>();
    //list of all tiles the player can not see
    private List<Tiles> m_notInSightTiles = new List<Tiles>();
    
    private void Awake()
    {
        //set default values
        mapOffsetX = 0;
        mapOffsetY = 0;

        mapWidth = map.width;
        mapHeight = map.height;

        m_texture = new Texture2D(0, 0);

        //set the offsets dependning on if the map size
        if (mapWidth > mapHeight)
        {
            mapOffsetY = (int)((mapWidth - mapHeight) * 0.5f);
        }
        else if (mapWidth < mapHeight)
        {
            mapOffsetX = (int)((mapHeight - mapWidth) * 0.5f);
        }

        //set the texture size keeping it a perfect square
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
    }

    public void Update()
    {
        //always update our texture
        SetUpTexture();
    }

    /*
    * SetUpTexture
    * public void function
    * 
    * this goes through all the steps to set up our texture
    * 
    * @returns nothing
    */
    public void SetUpTexture()
    {
        //make sure we are doing this for the right player
        m_activeplayer = gameManagment.activePlayer;
            
        //add tiles in first
        ApplyTileTint();

        //add in the units
        AddUnits();

        //set up some texture values so it looks decent
        m_texture.filterMode = FilterMode.Point;
        m_texture.wrapMode = TextureWrapMode.Clamp;

        //apply the changes to our texture
        m_texture.Apply();

        //set mini map texture to our custom made texture
        miniMap.texture = m_texture;
    }

    /*
    * AddUnits
    * private void function
    * 
    * this goes through all units in the scene adding them to 
    * our mini map texture
    * 
    * @returns nothing
    */
    private void AddUnits()
    {
        Color color;

        //we add in the active player colours first 
        if (m_activeplayer.playerID == 0)
        {
            color = new Color(0, 0, 1);
            AddUnitColor(color, m_activeplayer, false);
        }

        if (m_activeplayer.playerID == 1)
        {
            color = new Color(1, 0, 0);
            AddUnitColor(color, m_activeplayer, true);
        }

        //loop through all remainding players and add in there units
        for (int i = 0; i < gameManagment.players.Count; i++)
        {
            //make sure the player we are looking at is not the active player
            if (gameManagment.players[i].playerID != m_activeplayer.playerID)
            {
                //go through that players units
                for (int u = 0; u < gameManagment.players[i].units.Count; u++)
                {
                    //only add in that unit if it is in sight of the active player
                    if (gameManagment.players[i].units[u].inSight)
                    {
                        //if active player is player one just add in the unit
                        if (m_activeplayer.playerID == 0)
                        {
                            if (gameManagment.players[i].playerID == 1)
                            {
                                if (gameManagment.players[i].units[u] != null)
                                {
                                    m_texture.SetPixel((int)((gameManagment.players[i].units[u].transform.position.x + 0.5f) + mapOffsetY),
                                                        (int)((gameManagment.players[i].units[u].transform.position.z + 0.5f) + mapOffsetX),
                                                        new Color(1, 0, 0));
                                }
                            }
                        }
                        //if it is player two things get a bit more tricky as we need to flip values so player twos appropriate rotation
                        else if(m_activeplayer.playerID == 1)
                        {
                            if (gameManagment.players[i].playerID == 0)
                            {
                                if (mapOffsetX == mapOffsetY)
                                {
                                    m_texture.SetPixel((int)(map.width - ((gameManagment.players[i].units[u].transform.position.x + 0.5f))),
                                                        (int)(map.height - ((gameManagment.players[i].units[u].transform.position.z + 0.5f))),
                                                        new Color(0, 0, 1));
                                }
                                else if (mapOffsetX < mapOffsetY)
                                {
                                    m_texture.SetPixel((int)(map.width - ((gameManagment.players[i].units[u].transform.position.x + 0.5f) + mapOffsetY)),
                                                        (int)(map.height - ((gameManagment.players[i].units[u].transform.position.z + 0.5f) - (mapOffsetY * 2))),
                                                        new Color(0, 0, 1));
                                }
                                else if (mapOffsetX > mapOffsetY)
                                {
                                    m_texture.SetPixel((int)(map.width - ((gameManagment.players[i].units[u].transform.position.x + 0.5f) - (mapOffsetX * 2))),
                                                        (int)(map.height - ((gameManagment.players[i].units[u].transform.position.z + 0.5f) + mapOffsetX)),
                                                        new Color(0, 0, 1));
                                }
                            }
                        }
                    }
                }
            }
        }

        //apply all changes made to the texture
        m_texture.Apply();
    }

    /*
    * ApplyTileTint
    * private void function
    * 
    * this starts off the adding of tiles to the texture image
    * 
    * @returns nothing
    */
    private void ApplyTileTint()
    {
        //first set values to default
        List<Tiles> holder2 = new List<Tiles>();
        m_notInSightTiles.Clear();
        m_sightTiles.Clear();

        //go through and gather all tiles the player can "see"
        for (int i = 0; i < m_activeplayer.units.Count; i++)
        {
            if (m_activeplayer.units[i] != null)
            {
                holder2 = GetArea.GetAreaOfAttack(map.GetTileAtPos(m_activeplayer.units[i].transform.position), (int)m_activeplayer.units[i].AOV, map);
                for (int u = 0; u < holder2.Count; u++)
                {
                    if (CheckInSightTiles(holder2[u].indexPos) == false)
                    {
                        m_sightTiles.Add(holder2[u]);
                    }
                }
            }
        }

        //set not in sight tiles to be equal to all tiles
        List<Tiles> holder = map.mapTiles;

        //in all the not in sight tiles
        for (int i = 0; i < holder.Count; i++)
        {
            m_notInSightTiles.Add(holder[i]);
        }

        //go through and remove all of the sight tiles from the not in sight tiles
        for (int i = 0; i < m_sightTiles.Count; i++)
        {
            m_notInSightTiles.Remove(m_sightTiles[i]);
        }

        //function to apply the tiles to the texture
        MakeMapTextureOfTiles();
    }

    /*
    * ApplyTileTint
    * private bool function
    * 
    * goes through all current sight tiles to check if the value passed in
    * is already in our list or not
    * 
    * @param int index - value we are searching for
    * @returns bool - returns false if it is not in the list
    */
    private bool CheckInSightTiles(int index)
    {
        if (m_sightTiles.Count == 0)
        {
            return false;
        }

        for (int i = 0; i < m_sightTiles.Count; i++)
        {
            if (m_sightTiles[i].indexPos == index)
            {
                return true;
            }
        }

        return false;
    }

    /*
    * AddUnitColor
    * private void function
    * 
    * this adds in the units into the texture
    * 
    * @param Color color - the color we want to set the units position in the texture to
    * @param BasePlayer player - the players whos unit colours we are setting
    * @param bool swap - if we need to invert where we are placing them on the texture or not
    * 
    * @returns bool - returns false if it is not in the list
    */
    private void AddUnitColor(Color color, BasePlayer player, bool swap)
    {
        if (swap)
        {
            if (mapOffsetX == mapOffsetY)
            {
                for (int i = 0; i < player.units.Count; i++)
                {
                    m_texture.SetPixel((int)(map.width - ((player.units[i].transform.position.x + 0.5f) + mapOffsetY)),
                                (int)(map.height - ((player.units[i].transform.position.z + 0.5f) + mapOffsetX)),
                                color);
                }
            }
            else if (mapOffsetX < mapOffsetY)
            {
                for (int i = 0; i < player.units.Count; i++)
                {
                    m_texture.SetPixel((int)(map.width - ((player.units[i].transform.position.x + 0.5f) + mapOffsetY)),
                                (int)(map.height - ((player.units[i].transform.position.z + 0.5f) - (mapOffsetY * 2))),
                                color);
                }
            }
            else if (mapOffsetX > mapOffsetY)
            {
                for (int i = 0; i < player.units.Count; i++)
                {
                    m_texture.SetPixel((int)(map.width - ((player.units[i].transform.position.x + 0.5f) - (mapOffsetX * 2))),
                                (int)(map.height - ((player.units[i].transform.position.z + 0.5f) + mapOffsetX)),
                                color);
                }
            }
        }
        else
        {
            for (int i = 0; i < player.units.Count; i++)
            {
                m_texture.SetPixel((int)((player.units[i].transform.position.x + 0.5f) + mapOffsetY), (int)((player.units[i].transform.position.z + 0.5f) + mapOffsetX), color);
            }
        }
    }

    /*
    * MakeMapTextureOfTiles
    * public void function
    * 
    * this goes through all the sight and not in sight tiles and populates the texture with them
    * in sight tiles have there "default" colouring the same as the map preview colours and not in sight
    * tiles are all tinted a darker colour
    * 
    * @returns bool - returns false if it is not in the list
    */
    public void MakeMapTextureOfTiles()
    {
        //start off by seting the hole texture to black
        SetTextureToBlack();

        //if active player one no fancy shit needed just add them in
        if (m_activeplayer.playerID == 0)
        {
            for (int i = 0; i < m_sightTiles.Count; i++)
            {
                m_texture.SetPixel((int)(m_sightTiles[i].transform.position.x + mapOffsetY), 
                            (int)(m_sightTiles[i].transform.position.z + mapOffsetX), 
                            GetColor(m_sightTiles[i].tileType));
            }

            for (int i = 0; i < m_notInSightTiles.Count; i++)
            {

                m_texture.SetPixel((int)(m_notInSightTiles[i].transform.position.x + mapOffsetY),
                            (int)(m_notInSightTiles[i].transform.position.z + mapOffsetX),
                            GetColorTint(m_notInSightTiles[i].tileType));
            }
        }


        //if active player is player two we need to invert the times to match up with the camera view
        //thus we need to make many checks to make sure we place the colours in the right spo
        if (m_activeplayer.playerID == 1)
        {
            if (mapOffsetX == mapOffsetY)
            {
                for (int i = 0; i < m_sightTiles.Count; i++)
                {
                    m_texture.SetPixel((int)(map.width - ((m_sightTiles[i].transform.position.x + 0.5f))),
                               (int)(map.height - ((m_sightTiles[i].transform.position.z + 0.5f))),
                                GetColor(m_sightTiles[i].tileType));
                }

                for (int i = 0; i < m_notInSightTiles.Count; i++)
                {
                    m_texture.SetPixel((int)(map.width - ((m_notInSightTiles[i].transform.position.x + 0.5f))),
                               (int)(map.height - ((m_notInSightTiles[i].transform.position.z + 0.5f))),
                                GetColorTint(m_notInSightTiles[i].tileType));
                }

            }
            else if (mapOffsetY > mapOffsetX)
            {
                for (int i = 0; i < m_sightTiles.Count; i++)
                {
                    m_texture.SetPixel((int)(map.width - ((m_sightTiles[i].transform.position.x + 0.5f) + mapOffsetY)),
                               (int)(map.height - ((m_sightTiles[i].transform.position.z + 0.5f) - (mapOffsetY * 2))),
                                GetColor(m_sightTiles[i].tileType));
                }

                for (int i = 0; i < m_notInSightTiles.Count; i++)
                {
                    m_texture.SetPixel((int)(map.width - ((m_notInSightTiles[i].transform.position.x + 0.5f) + mapOffsetY)),
                               (int)(map.height - ((m_notInSightTiles[i].transform.position.z + 0.5f) - (mapOffsetY * 2))),
                                GetColorTint(m_notInSightTiles[i].tileType));
                }
            }
            else if (mapOffsetX > mapOffsetY)
            {
                for (int i = 0; i < m_sightTiles.Count; i++)
                {
                    m_texture.SetPixel((int)(map.width - ((m_sightTiles[i].transform.position.x + 0.5f) - (mapOffsetX * 2))),
                               (int)(map.height - ((m_sightTiles[i].transform.position.z + 0.5f) + mapOffsetX)),
                                GetColor(m_sightTiles[i].tileType));
                }

                for (int i = 0; i < m_notInSightTiles.Count; i++)
                {
                    m_texture.SetPixel((int)(map.width - ((m_notInSightTiles[i].transform.position.x + 0.5f) - (mapOffsetX * 2))),
                               (int)(map.height - ((m_notInSightTiles[i].transform.position.z + 0.5f) + mapOffsetX)),
                                GetColorTint(m_notInSightTiles[i].tileType));
                }
            }
        }
        
        m_texture.Apply();
    }

    /*
    * SetTextureToBlack
    * private void function
    * 
    * sets the tecture to be qual to plack before we paint on our map
    * 
    * @param int num - the amount we want to loop through painting them texture black
    * @returns nothing
    */
    private void SetTextureToBlack()
    {
        Color color = new Color(0, 0, 0);

        for (int x = 0; x < m_texture.width; x++)
        {
            for (int y = 0; y < m_texture.height; y++)
            {
                m_texture.SetPixel(x, y, color);
            }
        }

        m_texture.Apply();
    }

    /*
    * GetColorTint
    * private Color function
    * 
    * this function takes in a tile type thenpasses that into a switch statmeant,
    * depending on the tile type passed in it will pas out an apporpriate color
    * 
    * @param eTileType type - the current tile type we wish to paint in to our map preview texture
    * @returns Color - used to paint in our map preview texture
    */
    private Color GetColorTint(eTileType type)
    {
        switch (type)
        {
            case eTileType.NORMAL:
                return new Color(0.35f, 0.2f, 0.15f);

            case eTileType.DAMAGE:
                return new Color(0, 0.4f, 0);

            case eTileType.DEFENSE:
                return new Color(0.23f, 0.06f, 0);

            case eTileType.IMPASSABLE:
                return new Color(0f, 0f, 0f);

            case eTileType.NULL:
                return new Color(0, 0, 0);

            case eTileType.DEBUGGING:
                return new Color(0, 0, 0);

            case eTileType.PLACABLEDEFENSE:
                return new Color(0.23f, 0.06f, 0);

            case eTileType.PLACABLETRAP:
                return new Color(0, 0.4f, 0);
        }

        Debug.LogError("NOT VALID TYPE PASSED");
        return new Color(0, 0, 0);
    }

    /*
    * GetColor
    * private Color function
    * 
    * this function takes in a tile type then passes that into a switch statmeant,
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

        Debug.LogError("NOT VALID TYPE PASSED");
        return new Color(0, 0, 0);
    }
}

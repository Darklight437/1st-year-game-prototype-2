using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public Map map;
    
    public RawImage miniMap;
    public GameManagment gameManagment;

    int mapWidth = 0;
    int mapHeight = 0;
    int mapOffsetX = 0;
    int mapOffsetY = 0;

    private BasePlayer m_activeplayer;

    private Texture2D m_texture;

    private List<Tiles> m_sightTiles = new List<Tiles>();
    private List<Tiles> m_notInSightTiles = new List<Tiles>();

    private void Awake()
    {
        mapOffsetX = 0;
        mapOffsetY = 0;

        mapWidth = map.width;
        mapHeight = map.height;

        m_texture = new Texture2D(0, 0);

        if (mapWidth > mapHeight)
        {
            mapOffsetY = (int)((mapWidth - mapHeight) * 0.5f);
        }
        else if (mapWidth < mapHeight)
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
    }

    public void Update()
    {
        SetUpTexture();
    }

    public void SetUpTexture()
    {
        m_activeplayer = gameManagment.activePlayer;
            
        ApplyTileTint();

        AddUnits();

        m_texture.filterMode = FilterMode.Point;
        m_texture.wrapMode = TextureWrapMode.Clamp;

        m_texture.Apply();

        miniMap.texture = m_texture;
    }

    private void AddUnits()
    {
        Color color;

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

        for (int i = 0; i < gameManagment.players.Count; i++)
        {
            if (gameManagment.players[i].playerID != m_activeplayer.playerID)
            {
                for (int u = 0; u < gameManagment.players[i].units.Count; u++)
                {
                    if (gameManagment.players[i].units[u].inSight)
                    {
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

        m_texture.Apply();
    }

    private void ApplyTileTint()
    {
        List<Tiles> holder2 = new List<Tiles>();
        m_notInSightTiles.Clear();
        m_sightTiles.Clear();

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

        List<Tiles> holder = map.mapTiles;

        for (int i = 0; i < holder.Count; i++)
        {
            m_notInSightTiles.Add(holder[i]);
        }

        for (int i = 0; i < m_sightTiles.Count; i++)
        {
            m_notInSightTiles.Remove(m_sightTiles[i]);
        }

        MakeMapTextureOfTiles();
    }

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

    public void MakeMapTextureOfTiles()
    {
        SetTextureToBlack();

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

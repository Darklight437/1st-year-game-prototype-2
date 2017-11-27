using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Statistics", menuName = "Inventory/Statistics", order = 1)]
public class Statistics : ScriptableObject
{
    //reference to the armour function
    public AnimationCurve armourCurve = AnimationCurve.Linear(0,0,4,50);

    //amount of health gained when a healing tile is stepped on
    public float tileHealthGained = 500.0f;

    //amount of damage a trap tile does when stepped on
    public float trapTileDamage = 100.0f;

    public float shrinkZoneDamage;

    //the amount and list of normal tiles that are still useable for random logic
    public int normalTileTypeAmount;
    private List<int> m_normalTileVariantUnused = new List<int>();

    //the amount and list of damage tiles that are still useable for random logic
    public int damageTileTypeAmount;
    private List<int> m_damageTileVariantUnused = new List<int>();

    //the amount and list of impassable tiles that are still useable for random logic
    public int impassableTileTypeAmount;
    private List<int> m_impassableTileVariantUnused = new List<int>();

    //the amount and list of defense tiles that are still useable for random logic
    public int defenseTileTypeAmount;
    private List<int> m_defenseTileVariantUnused = new List<int>();

    //the amount of placable defense tiles
    public int placableDefenseTileAmount;
    private List<int> m_placabaleDefenseTileVariantUnsed = new List<int>();

    //the amount of placable trap Tiles
    public int placableTrapTileAmount;
    private List<int> m_placableTrapTileVariantUnsed = new List<int>();

    /*
    * RandomTileNum 
    * int function
    * 
    * this is used for shuffle bag logic random number generation used for tile type varient generation
    * 
    * @returns int - returns an int used as an indext to get a new random tile
    * @Author Callum Dunstone
    */
    public int RandomTileNum(eTileType type)
    {
        RestockTileVarientsUnsused();
        int num = 0;
        
        switch (type)
        {
            case eTileType.NORMAL:
                num = Random.Range(0, m_normalTileVariantUnused.Count);
                num = m_normalTileVariantUnused[num];
                m_normalTileVariantUnused.Remove(num);
                return num;

            case eTileType.DAMAGE:
                num = Random.Range(0, m_damageTileVariantUnused.Count);
                num = m_damageTileVariantUnused[num];
                m_damageTileVariantUnused.Remove(num);
                return num;

            case eTileType.DEFENSE:
                num = Random.Range(0, m_defenseTileVariantUnused.Count);
                num = m_defenseTileVariantUnused[num];
                m_defenseTileVariantUnused.Remove(num);
                return num;

            case eTileType.IMPASSABLE:
                num = Random.Range(0, m_impassableTileVariantUnused.Count);
                num = m_impassableTileVariantUnused[num];
                m_impassableTileVariantUnused.Remove(num);
                return num;

            case eTileType.NULL:
                return 0;

            case eTileType.DEBUGGING:
                return 0;

            case eTileType.PLACABLEDEFENSE:
                num = Random.Range(0, m_placabaleDefenseTileVariantUnsed.Count);
                num = m_placabaleDefenseTileVariantUnsed[num];
                m_placabaleDefenseTileVariantUnsed.Remove(num);
                return num;

            case eTileType.PLACABLETRAP:
                num = Random.Range(0, m_placableTrapTileVariantUnsed.Count);
                num = m_placableTrapTileVariantUnsed[num];
                m_placableTrapTileVariantUnsed.Remove(num);
                return num;

        }

        Debug.LogError("INVALID TILE TYPE PASSED INTO STATISTICS RANDOMTILENUM");
        return 0;
    }

    /*
    * RestockTileVarientsUnsused 
    * void function
    * 
    * this checks all the list used for the shuffle bag to see if any has run out and restocks it if so
    * 
    * @returns void
    * @Author Callum Dunstone
    */
    public void RestockTileVarientsUnsused()
    {
        if (m_normalTileVariantUnused.Count == 0)
        {
            for (int i = 0; i < normalTileTypeAmount; i++)
            {
                m_normalTileVariantUnused.Add(i);
            }
        }

        if (m_damageTileVariantUnused.Count == 0)
        {
            for (int i = 0; i < damageTileTypeAmount; i++)
            {
                m_damageTileVariantUnused.Add(i);
            }
        }

        if (m_defenseTileVariantUnused.Count == 0)
        {
            for (int i = 0; i < defenseTileTypeAmount; i++)
            {
                m_defenseTileVariantUnused.Add(i);
            }
        }

        if (m_impassableTileVariantUnused.Count == 0)
        {
            for (int i = 0; i < impassableTileTypeAmount; i++)
            {
                m_impassableTileVariantUnused.Add(i);
            }
        }

        if (m_placabaleDefenseTileVariantUnsed.Count == 0)
        {
            for (int i = 0; i < placableDefenseTileAmount; i++)
            {
                m_placabaleDefenseTileVariantUnsed.Add(i);
            }
        }

        if (m_placableTrapTileVariantUnsed.Count == 0)
        {
            for (int i = 0; i < placableTrapTileAmount; i++)
            {
                m_placableTrapTileVariantUnsed.Add(i);
            }
        }
    }

    //the amount and list of normal tiles that are still useable for random logic
    public int normalChunkTypeAmount;
    private List<int> m_normalChunkVariantUnused = new List<int>();

    //the amount and list of damage tiles that are still useable for random logic
    public int damageChunkTypeAmount;
    private List<int> m_damageChunkVariantUnused = new List<int>();

    //the amount and list of impassable tiles that are still useable for random logic
    public int impassableChunkTypeAmount;
    private List<int> m_impassableChunkVariantUnused = new List<int>();

    //the amount and list of defense tiles that are still useable for random logic
    public int defenseChunkTypeAmount;
    private List<int> m_defenseChunkVariantUnused = new List<int>();

    public int RandomChunkNum(eChunkTypes type)
    {
        RestockChunkLists();

        int num = 0;

        switch (type)
        {
            case eChunkTypes.NORMAL:
                num = Random.Range(0, m_normalChunkVariantUnused.Count);
                num = m_normalChunkVariantUnused[num];
                m_normalChunkVariantUnused.Remove(num);
                return num;

            case eChunkTypes.DAMAGE:
                num = Random.Range(0, m_damageChunkVariantUnused.Count);
                num = m_damageChunkVariantUnused[num];
                m_damageChunkVariantUnused.Remove(num);
                return num;

            case eChunkTypes.DEFENSE:
                num = Random.Range(0, m_defenseChunkVariantUnused.Count);
                num = m_defenseChunkVariantUnused[num];
                m_defenseChunkVariantUnused.Remove(num);
                return num;

            case eChunkTypes.IMPASSABLE:
                num = Random.Range(0, m_impassableChunkVariantUnused.Count);
                num = m_impassableChunkVariantUnused[num];
                m_impassableChunkVariantUnused.Remove(num);
                return num;
        }

        Debug.LogError("INVALID TILE TYPE PASSED INTO STATISTICS RANDOMCHUNKNUM");
        return num;
    }

    public void RestockChunkLists()
    {
        if (m_normalChunkVariantUnused.Count == 0)
        {
            for (int i = 0; i < normalChunkTypeAmount; i++)
            {
                m_normalChunkVariantUnused.Add(i);
            }
        }

        if (m_damageChunkVariantUnused.Count == 0)
        {
            for (int i = 0; i < damageChunkTypeAmount; i++)
            {
                m_damageChunkVariantUnused.Add(i);
            }
        }

        if (m_defenseChunkVariantUnused.Count == 0)
        {
            for (int i = 0; i < defenseChunkTypeAmount; i++)
            {
                m_defenseChunkVariantUnused.Add(i);
            }
        }

        if (m_impassableChunkVariantUnused.Count == 0)
        {
            for (int i = 0; i < impassableChunkTypeAmount; i++)
            {
                m_impassableChunkVariantUnused.Add(i);
            }
        }
    }
}

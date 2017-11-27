using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class TileModifierCommand
* child class of UnitCommand
* 
* executes a Unit's method of altering different tiles
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class TileModifierCommand : UnitCommand
{
    //custom type for different types of modifications
    public enum eModifyType
    {
        HEALING = 0,
        TRAP = 1,
        DEFENSE = 2,
    }

    //the type of modification to apply
    public eModifyType modifyType = eModifyType.HEALING;

    //timer is a timer
    private float m_timer;

    //the wait time between tile actions
    private float m_waitTime = 0.5f;

    //the count of how many times we have done animations
    private float m_count;

    private bool m_playAnim;

    /*
    * TileModifierCommand()
    * 
    * constructor, specifies the target tile and callback
    * 
    * @param Unit u - the unit that made this command
    * @param VoidFunc scb - the callback to use when finished
    * @param VoidFunc fcb - the callback to use when failed
    * @param Tiles st - the first tile selected
    * @param Tiles et - the last tile selected
    */
    public TileModifierCommand(Unit u, VoidFunc scb, VoidFunc fcb, Tiles st, Tiles et) : base(u, scb, fcb, st, et)
    {
        if (unit.ArtLink != null)
        {
            unit.ArtLink.SetTrigger("SAttack");
        }

        m_playAnim = false;
    }


    /*
    * Update
    * overrides UnitCommand's Update()
    * 
    * called once per frame while the command is active
    * 
    * @returns void
    */
    public override void Update()
    {
        if (modifyType == eModifyType.HEALING &&
            endTile.tileType != eTileType.IMPASSABLE && 
            (endTile.isHealing == false))
        {
            endTile.IsHealing(true, unit);
        }
        else if(modifyType == eModifyType.HEALING)
        {
            if (endTile.isHealing == true)
            {
                failedCallback();
                return;
            }

            if (endTile.tileType == eTileType.IMPASSABLE)
            {
                failedCallback();
                return;
            }

            if (endTile.unit != null && endTile.unit.playerID != unit.playerID)
            {
                failedCallback();
                return;
            }
        }

        if (m_playAnim == false && unit.ArtLink != null)
        {
            unit.ArtLink.SetBool("ActionsAvailable", false);
            m_playAnim = true;
        }

        m_timer += Time.deltaTime;

        if (modifyType == eModifyType.TRAP && m_timer >= m_waitTime && m_count < 3)
        {
            endTile.SandExplosion();
            m_count++;
            m_timer = 0;
            return;
        }

        if (modifyType == eModifyType.DEFENSE && m_timer >= m_waitTime && m_count < 3)
        {
            if (m_count == 0)
            {
                //shoot flare
            }

            if (m_count == 1)
            {
                endTile.TileAirDrop();
                m_waitTime = 0.1f;
            }

            if (m_count == 2)
            {
                endTile.SandExplosion();
                m_waitTime = 0.5f;
            }

            m_count++;
            m_timer = 0;
            return;
        }

        if ((modifyType == eModifyType.TRAP || modifyType == eModifyType.DEFENSE) && m_count < 3)
        {
            return;
        }

        if (modifyType != eModifyType.HEALING)
        {
            //can't modify a wall or change a tile being stood on
            if (endTile.tileType != eTileType.NORMAL)
            {
                failedCallback();
                return;
            }

            //determine what to do
            switch (modifyType)
            {
                case eModifyType.TRAP: endTile.tileType = eTileType.PLACABLETRAP; endTile.GenerateRandomTileVariant(); break;
                case eModifyType.DEFENSE: endTile.tileType = eTileType.PLACABLEDEFENSE; endTile.GenerateRandomTileVariant(); break;
            }
        }

        unit.hasAttacked = true;
        unit.movementPoints = 0;

        if (unit.ArtLink != null)
        {
            unit.ArtLink.SetBool("ActionsAvailable", false);
        }

        if (endTile.unit != null)
        {
            //this is a healing tile
            if (endTile.IsHealing(false, unit))
            {
                endTile.unit.Heal(GameManagment.stats.tileHealthGained);
            }

            //this is a defensive tile
            if (endTile.tileType == eTileType.PLACABLEDEFENSE)
            {
                //defensive buff
                endTile.unit.armour = endTile.unit.baseArmour + 1;
            }
            else
            {
                //remove the defensive buff
                endTile.unit.armour = endTile.unit.baseArmour;
            }
        }

        successCallback();
    }
}

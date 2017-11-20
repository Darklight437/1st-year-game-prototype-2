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
        //can't modify a wall or change a tile being stood on
        if (endTile.tileType != eTileType.NORMAL)
        {
            failedCallback();
            return;
        }

        //determine what to do
        switch (modifyType)
        {
            case eModifyType.HEALING: endTile.IsHealing = true; break;
            case eModifyType.TRAP: endTile.tileType = eTileType.PLACABLETRAP; endTile.GenerateRandomTileVariant(); break;
            case eModifyType.DEFENSE: endTile.tileType = eTileType.PLACABLEDEFENSE; endTile.GenerateRandomTileVariant(); break;
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
            if (endTile.IsHealing)
            {
                endTile.unit.Heal(GameManagment.stats.tileHealthGained);
            }

            //this is a trap tile, it could kill the unit
            if (endTile.tileType == eTileType.PLACABLETRAP)
            {
                if (endTile.unit.ArtLink != null)
                {
                    endTile.unit.ArtLink.SetTrigger("TakeDamage");
                }

                endTile.unit.Defend(GameManagment.stats.trapTileDamage);
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

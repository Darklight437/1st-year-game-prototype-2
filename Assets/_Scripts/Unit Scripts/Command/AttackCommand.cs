using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class AttackCommand
* child class of UnitCommand
* 
* executes a Unit's attack routine
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class AttackCommand : UnitCommand
{

    public float attackTimer = 0.0f;

    //reference to the map
    public Map map = null;

    /*
    * AttackCommand()
    * 
    * constructor, specifies the target tile and callbacks
    * 
    * @param Unit u - the unit that made this command
    * @param VoidFunc scb - the callback to use when finished
    * @param VoidFunc fcb - the callback to use when failed
    * @param Tiles st - the first tile selected
    * @param Tiles et - the last tile selected
    */
    public AttackCommand(Unit u, VoidFunc scb, VoidFunc fcb, Tiles st, Tiles et) : base(u, scb, fcb, st, et)
    {
        //find the map component
        map = GameObject.FindObjectOfType<Map>();

        // Attacking the enemy unit Anim
        if (unit.ArtLink != null)
        {
            unit.ArtLink.SetTrigger("Attack");
        }
        //Enemy Unit taking damage Anim
        if (unit.ArtLink != null)
        {
            et.unit.ArtLink.SetTrigger("TakeDamage");
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
        unit.hasAttacked = true;
        unit.movementPoints = 0;

        //count-down the attack timer
        attackTimer -= Time.deltaTime;

        if (attackTimer < 0.0f)
        {
            attackTimer = 0.0f;
        }
        else
        {
            return;
        }

        Unit defendingUnit = endTile.unit;

        //if the defending unit exists
        if (defendingUnit != null)
        {
            unit.Attack(defendingUnit);
            successCallback();
        }
        else
        {
            failedCallback();
        }
        if (unit.ArtLink != null)
        {
            unit.ArtLink.SetBool("ActionsAvailable", false);
        }
    }
}

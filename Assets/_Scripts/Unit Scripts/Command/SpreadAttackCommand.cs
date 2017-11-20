using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class SpreadAttackCommand
* child class of UnitCommand
* 
* executes a Unit's spread attack routine
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class SpreadAttackCommand : UnitCommand
{

    public float attackTimer = 0.0f;
    public float attackRadius = 0.0f;

    //reference to the map
    public Map map = null;

    /*
    * SpreadAttackCommand()
    * 
    * constructor, specifies the target tile and callbacks
    * 
    * @param Unit u - the unit that made this command
    * @param VoidFunc scb - the callback to use when finished
    * @param VoidFunc fcb - the callback to use when failed
    * @param Tiles st - the first tile selected
    * @param Tiles et - the last tile selected
    */
    public SpreadAttackCommand(Unit u, VoidFunc scb, VoidFunc fcb, Tiles st, Tiles et) : base(u, scb, fcb, st, et)
    {
        //find the map component
        map = GameObject.FindObjectOfType<Map>();

        // Attacking the enemy unit Anim
        if (unit.ArtLink != null)
        {
            unit.ArtLink.SetTrigger("Attack");
        }
        //Enemy Unit taking damage Anim
        if (et.unit != null && et.unit.ArtLink != null)
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

        int maxDistance = Mathf.CeilToInt(attackRadius);

        //get the surrounding tiles, considering obstacles
        List<Tiles> area = GetArea.GetAreaOfAttack(endTile, maxDistance, map);

        //get the size of the area tiles list
        int areaSize = area.Count;

        //iterate through all of the tiles in the area, applying damage to all
        for (int i = 0; i < areaSize; i++)
        {
            Unit defendingUnit = area[i].unit;

            //if the defending unit exists and isn't a friendly unit
            if (defendingUnit != null && defendingUnit.playerID != unit.playerID)
            {
                //relative vector from the start to the end
                Vector3 relative = area[i].pos - endTile.pos;

                //manhattan distance
                int manhattDistance = (int)(relative.x + relative.z);

                float ratio = 1 - ((float)(manhattDistance - 1)  / (float)maxDistance);

                unit.Attack(defendingUnit, ratio);

            }
            
        }
        if (unit.ArtLink != null)
        {
            unit.ArtLink.SetBool("ActionsAvailable", false);
        }

        //reset the explosion
        ParticleLibrary.explosionSystem.transform.position = endTile.transform.position;
        ParticleLibrary.explosionSystem.time = 0.0f;
        ParticleLibrary.explosionSystem.Play();


        successCallback();
    }
}

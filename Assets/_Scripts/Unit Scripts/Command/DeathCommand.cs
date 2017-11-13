using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class DeathCommand
* child class of UnitCommand
* 
* executes a Unit's dying routine
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class DeathCommand : UnitCommand
{

    public float deathTimer = 0.0f;

    public void Start()
    {

    }


    /*
    * DeathCommand()
    * 
    * constructor, specifies the target tile and callback
    * 
    * @param Unit u - the unit that made this command
    * @param VoidFunc scb - the callback to use when finished
    * @param VoidFunc fcb - the callback to use when failed
    * @param Tiles st - the first tile selected
    * @param Tiles et - the last tile selected
    */
    public DeathCommand(Unit u, VoidFunc scb, VoidFunc fcb, Tiles st, Tiles et) : base(u, scb, fcb, st, et)
    {
        if (unit.ArtLink != null)
        {
            unit.ArtLink.SetTrigger("Death");
        }
        //disconnect the unit from the grid
        startTile.unit = null;
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
        deathTimer -= Time.deltaTime;

        if (deathTimer <= 0.0f)
        {
            deathTimer = 0.0f;

            successCallback();
            GameObject.Destroy(unit.gameObject);
        }
    }
}

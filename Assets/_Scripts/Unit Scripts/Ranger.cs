using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class Ranger
* child class of Unit
* 
* strong from a distance, weak close-up, fast
* 
* author: Daniel Witt, Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class Ranger : Unit
{
    //the amount of tiles away an attack can reach
    public float splashRange = 3.0f;

    /*
    * Execute 
    * overrides function Unit's Execute(GameManagment.eActionType actionType, int tileX, int tileY)
    * 
    * adds a command of the specified type to the unit
    * 
    * @param GameManagement.eActionType actionType - the type of action to execute
    * @param Tiles st - the first tile selected
    * @param Tiles et - the last tile selected
    * @param UnitCommand.VoidFunc callback - function reference to invoke if the command completes
    * @returns void
    */
    public override void Execute(GameManagment.eActionType actionType, Tiles st, Tiles et, UnitCommand.VoidFunc callback)
    {
        //create a list of function references to execute
        UnitCommand.VoidFunc callstack = OnCommandFinish;
        callstack += callback;

        //movement command
        if (actionType == GameManagment.eActionType.MOVEMENT)
        {
            MoveCommand mc = new MoveCommand(this, callstack, OnCommandFailed, st, et);

            commands.Add(mc);
        }
        //attack command
        else if (actionType == GameManagment.eActionType.ATTACK)
        {
            if (hasAttacked == false)
            {
                SpreadAttackCommand ac = new SpreadAttackCommand(this, callstack, OnCommandFailed, st, et);

                ac.attackTimer = attackTime;
                ac.attackRadius = splashRange;

                commands.Add(ac);
            }
        }
        //dying command
        else if (actionType == GameManagment.eActionType.DEATH)
        {
            DeathCommand dc = new DeathCommand(this, callstack, null, st, null);

            dc.deathTimer = deathTime;

            commands.Add(dc);
        }
    }
}

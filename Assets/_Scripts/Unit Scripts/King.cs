using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class King
* child class of Unit
* 
* the most important unit in the game, if the king dies the player loses
* 
* author: Daniel Witt, Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class King : Unit
{
    //how close a unit has to be to be considered in damage boosting
    public int adjacentUnitRange = 3;

    //the amount of damage boosted when close enough to the king
    public float flatDamageRatio = 0.1f;

    //percentages of king damage addition givenn
    public float[] kingDamageRatios = new float[3] { 0.05f, 0.1f, 0.15f };

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
    public override void Execute(GameManagment.eActionType actionType, Tiles st, Tiles et, UnitCommand.VoidFunc callback, bool safeMove)
    {
        //create a list of function references to execute
        UnitCommand.VoidFunc callstack = OnCommandFinish;
        callstack += callback;

        //movement command
        if (actionType == GameManagment.eActionType.MOVEMENT)
        {
            
            MoveCommand mc = new MoveCommand(this, callstack, OnCommandFailed, st, et, safeMove);

            commands.Add(mc);
        }
        //attack command
        else if (actionType == GameManagment.eActionType.ATTACK)
        {
            AttackCommand ac = new AttackCommand(this, callstack, OnCommandFailed, st, et);

            ac.attackTimer = attackTime;

            commands.Add(ac);
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

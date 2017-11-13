using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
* class UnitCommand
* 
* base class for Unit action routine
* 
* eg. moving from one location to another, attacking, animations
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class UnitCommand
{

    //reference to the unit that owns this command
    public Unit unit = null;

    //delegate type
    public delegate void VoidFunc();

    //function reference to call when complete
    public VoidFunc successCallback = null;

    //function reference to call when failed
    public VoidFunc failedCallback = null;

    //reference to the starting tile
    public Tiles startTile = null;

    //reference to the ending tile
    public Tiles endTile = null;

    /*
    * UnitCommand()
    * 
    * constructor, specifies the target tile and callback
    * 
    * @param Unit u - the unit that made this command
    * @param VoidFunc scb - the callback to use when finished
    * @param VoidFunc fcb - the callback to use when failed
    * @param Tiles st - the first tile selected
    * @param Tiles et - the last tile selected
    */
    public UnitCommand(Unit u, VoidFunc scb, VoidFunc fcb, Tiles st, Tiles et)
    {
        unit = u;
        successCallback = scb;
        failedCallback = scb;
        startTile = st;
        endTile = et;
    }


    /*
    * Update
    * virtual function
    * 
    * called once per frame when active
    * 
    * @returns void
    */
    public virtual void Update()
    {
        
    }
	
}

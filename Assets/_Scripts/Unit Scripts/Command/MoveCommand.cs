using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class MoveCommand
* child class of UnitCommand
* 
* executes a Unit's movement routine
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class MoveCommand : UnitCommand
{
    //reference to the map
    public Map map = null;

    //list of tiles to follow
    private List<Tiles> m_tilePath = null;

    /*
    * MoveCommand()
    * 
    * constructor, specifies the target tile and callback
    * 
    * @param Unit u - the unit that made this command
    * @param VoidFunc scb - the callback to use when finished
    * @param VoidFunc fcb - the callback to use when failed
    * @param Tiles st - the first tile selected
    * @param Tiles et - the last tile selected
    */
    public MoveCommand(Unit u, VoidFunc scb, VoidFunc fcb, Tiles st, Tiles et) : base(u, scb, fcb, st, et)
    {
        if (unit.ArtLink != null)
        {
            unit.ArtLink.SetBool("IsWalking", true);
        }
        //find the map component
        map = GameObject.FindObjectOfType<Map>();
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
        if (m_tilePath == null)
        {
            //get the start and end of the path
            Tiles startingTile = map.GetTileAtPos(unit.transform.position);

            startingTile.unit = null;

            //get the tile path to follow
            m_tilePath = AStar.GetAStarPath(startingTile, endTile);

            //the path is clear, and the unit can move there
            if (m_tilePath.Count > 0 && m_tilePath.Count <= unit.movementPoints)
            {
                //subtract the path distance from the movement points
                unit.movementPoints -= m_tilePath.Count;

                startingTile.unit = null;
                endTile.unit = unit;
            }
            else
            {
                //Stop walking Anim
                if (unit.ArtLink != null)
                {
                    unit.ArtLink.SetBool("IsWalking", false);
                }

                //the path failed
                startingTile.unit = unit;
                failedCallback();
                return;
            }
        }

        //check if there is still a path to follow
        if (m_tilePath.Count > 0)
        {
            //get the next position to go to
            Tiles nextTile = m_tilePath[0];

            //the 3D target of the movement
            Vector3 target = new Vector3(nextTile.pos.x, 0.5f, nextTile.pos.z);

            Vector3 relative = target - unit.transform.position;

            if (relative.magnitude < unit.movementSpeed * Time.deltaTime)
            {
                //this is a healing tile, heal the unit
                if (nextTile.IsHealing)
                {
                    unit.Heal(GameManagment.stats.tileHealthGained);
                }

                //this is a trap tile, it could kill the unit
                if (nextTile.tileType == eTileType.PLACABLETRAP || nextTile.tileType == eTileType.DAMAGE)
                {
                    if (unit.ArtLink != null)
                    {
                        unit.ArtLink.SetTrigger("TakeDamage");
                    }

                    unit.Defend(GameManagment.stats.trapTileDamage);

                    //reset the explosion
                    ParticleLibrary.explosionSystem.transform.position = nextTile.transform.position;
                    ParticleLibrary.explosionSystem.time = 0.0f;
                    ParticleLibrary.explosionSystem.Play();

                    //an unexpected stop has occured, reset the target
                    endTile.unit = null;
                    nextTile.unit = unit;
                    
                    //regain most movemeantpoints back
                    unit.movementPoints += m_tilePath.Count - 1;
                    
                    if (unit.movementPoints < 0)
                    {
                        unit.movementPoints = 0;
                    }

                    //movement stops if a trap tile is hit
                    m_tilePath.Clear();

                    unit.transform.position = target;
                    successCallback();

                    return;
                }

                unit.transform.position = target;
                m_tilePath.RemoveAt(0);
            }
            else
            {
                unit.transform.position += relative.normalized * unit.movementSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (endTile.tileType == eTileType.PLACABLEDEFENSE || endTile.tileType == eTileType.DEFENSE)
            {
                //defensive buff
                endTile.unit.armour = endTile.unit.baseArmour + 1;
            }
            else
            {
                //remove the defensive buff
                endTile.unit.armour = endTile.unit.baseArmour;
            }
            successCallback();

            //Stop walking Anim
            if (unit.ArtLink != null)
            {
                unit.ArtLink.SetBool("IsWalking", false);
            }

            return;
        }
    }
}

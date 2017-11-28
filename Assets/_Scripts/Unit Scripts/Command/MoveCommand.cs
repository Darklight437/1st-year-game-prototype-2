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

    //reference to the game manager
    private GameManagment m_manager = null;

    //are we doing a safe or not safe move
    private bool isSafeMove;

    //this timmer gets set if the unit has stopped on a time
    private float m_timer;

    //wait time for standing still
    private float m_waitTime = 6;

    private bool m_finishedWaiting;

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
    public MoveCommand(Unit u, VoidFunc scb, VoidFunc fcb, Tiles st, Tiles et, bool safeMove) : base(u, scb, fcb, st, et)
    {
        if (unit.ArtLink != null)
        {
            unit.ArtLink.SetBool("IsWalking", true);
        }
        //find the map component
        map = GameObject.FindObjectOfType<Map>();

        //find the manager component
        m_manager = GameObject.FindObjectOfType<GameManagment>();

        isSafeMove = safeMove;

        m_timer = m_waitTime;
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
            if (isSafeMove)
            {
                m_tilePath = AStar.GetSafeAStarPath(startingTile, endTile, unit, GetArea.GetAreaOfSafeMoveable(startingTile, unit.movementPoints, map, unit));

            }
            else
            {
                m_tilePath = AStar.GetAStarPath(startingTile, endTile, unit);
            }

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

        m_timer += Time.deltaTime;

        //check if there is still a path to follow
        if (m_tilePath.Count > 0 && m_timer > m_waitTime)
        {
            if (m_finishedWaiting == false)
            {
                m_finishedWaiting = true;

                if (unit.ArtLink != null)
                {
                    unit.ArtLink.SetBool("IsWalking", true);
                }
            }

            //get the next position to go to
            Tiles nextTile = m_tilePath[0];

            //the 3D target of the movement
            Vector3 target = new Vector3(nextTile.pos.x, 0.15f, nextTile.pos.z);

            Vector3 relative = target - unit.transform.position;

            if (relative.magnitude < unit.movementSpeed * Time.deltaTime)
            {
                //this is a healing tile, heal the unit
                if (nextTile.IsHealing(false, unit))
                {
                    unit.Heal(GameManagment.stats.tileHealthGained);
                }

                //this is a trap tile, it could kill the unit
                if (nextTile.tileType == eTileType.PLACABLETRAP || nextTile.tileType == eTileType.DAMAGE)
                {
                    unit.Defend(GameManagment.stats.trapTileDamage);

                    //explosion is required
                    if (nextTile.tileType == eTileType.PLACABLETRAP)
                    {
                        //reset the explosion
                        ParticleLibrary.explosionSystem.transform.position = nextTile.transform.position;
                        ParticleLibrary.explosionSystem.time = 0.0f;
                        ParticleLibrary.explosionSystem.Play();
                    }

                    m_timer = 0;
                    m_finishedWaiting = false;

                    //Stop walking Anim
                    if (unit.ArtLink != null)
                    {
                        unit.ArtLink.SetBool("IsWalking", false);
                    }
                }

                unit.transform.position = target;
                m_tilePath.RemoveAt(0);

                
            }
            else
            {
                unit.transform.position += relative.normalized * unit.movementSpeed * Time.deltaTime;
            }
        }
        else if(m_timer > m_waitTime)
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

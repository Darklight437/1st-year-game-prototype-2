using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class AiPlayer
* child class of BasePlayer
* 
* object for a player that can be either a human or ai player
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class DynamicPlayer : BasePlayer
{

    //reference to both types of players to use
    private HumanPlayer m_human;

    [HideInInspector]
    public AiPlayer computer;

    //worst and the best statistical players
    public AiPlayer worstPlayer = null;
    public AiPlayer bestPlayer = null;

    // Use this for initialization
    new void Start()
    {
        m_human = gameObject.AddComponent<HumanPlayer>();
        computer = gameObject.AddComponent<AiPlayer>();

        //assign the units
        m_human.units = units;
        computer.units = units;

        computer.logicMachine = gameObject.GetComponent<FuzzyLogic>();

        //set the difficulty
        SetDifficulty(Settings.aiDifficulty);

        //mark the player as either a human or not
        isHuman = !Settings.playingWithAI;
    }


    /*
    * SetDifficulty 
    * 
    * interpolates all of the stats starting from the worst player to the best player
    * 
    * 0 = worst player stats
    * 0.5 = halfway between worst and best
    * 1 = best player stats
    * 
    * @param flaot difficulty - 0-1 interpolation value of the two ai players
    * @returns void
    */
    public void SetDifficulty(float difficulty)
    {
        computer.m_idleTimer = Mathf.Lerp(worstPlayer.m_idleTimer, bestPlayer.m_idleTimer, difficulty);
        computer.idleTime = Mathf.Lerp(worstPlayer.idleTime, bestPlayer.idleTime, difficulty);
        computer.initialThinkTime = Mathf.Lerp(worstPlayer.initialThinkTime, bestPlayer.initialThinkTime, difficulty);
        computer.thinkTime = Mathf.Lerp(worstPlayer.thinkTime, bestPlayer.thinkTime, difficulty);
        computer.advanceImportance = Mathf.Lerp(worstPlayer.advanceImportance, bestPlayer.advanceImportance, difficulty);
        computer.fleeImportance = Mathf.Lerp(worstPlayer.fleeImportance, bestPlayer.fleeImportance, difficulty);
        computer.attackImportance = Mathf.Lerp(worstPlayer.attackImportance, bestPlayer.attackImportance, difficulty);
        computer.healingImportance = Mathf.Lerp(worstPlayer.healingImportance, bestPlayer.healingImportance, difficulty);
        computer.effortImportance = Mathf.Lerp(worstPlayer.effortImportance, bestPlayer.effortImportance, difficulty);
        computer.groupImportance = Mathf.Lerp(worstPlayer.groupImportance, bestPlayer.groupImportance, difficulty);
        computer.kingBias = Mathf.Lerp(worstPlayer.kingBias, bestPlayer.kingBias, difficulty);
    }


    // Update is called once per frame
    new void Update()
    {
        m_human.playerID = playerID;
        computer.playerID = playerID;
    }


    /*
    * UpdateTurn 
    * overrides BasePlayers' UpdateTurn()
    * 
    * called once per frame while the player is active
    * 
    * @returns void
    */
    public override void UpdateTurn()
    {
        //update the ai component
        if (Settings.playingWithAI)
        {
            computer.UpdateTurn();
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
* class ConditionBehaviour
* child class of Behaviour
* 
* a behaviour tree node that executes checks and returns a response
* based on the evaluation of it's function reference
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class ConditionBehaviour : Behaviour
{
    [SerializeField]
    //function reference to execute when the behaviour node is called upon
    public TreeEvent action = null;

    /*
    * Execute 
    * overrides Behaviour's Update()
    * 
    * calls upon another object to run a function
    * 
    * @returns BehaviourReturn - depends on what the condition node is checking for
    */
    public override BehaviourReturn Execute()
    {
        if (action != null)
        {
            //run the action
            action.Invoke(tree);

            //the action will modify this value
            return tree.privateReturnValue;
        }
        else
        {
            //return nothing, the behaviour was improperly initialised
            return BehaviourReturn.NONE;
        }
    }
}

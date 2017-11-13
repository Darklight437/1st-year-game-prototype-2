using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//define a type that can be serialised
[System.Serializable]
public class TreeEvent : UnityEvent<BehaviourTree> { };

/*
* class ActionBehaviour
* child class of Behaviour
* 
* a behaviour tree node that's sole purpose is to execute a function
* and not affect the flow of the behaviour tree
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class ActionBehaviour : Behaviour
{
    [SerializeField]
    //function reference to execute when the behaviour node is called upon
    public TreeEvent action = null;

    /*
    * Execute 
    * overrides Behaviour's Update()
    * 
    * always returns success, calls upon another object to run a function
    * 
    * @returns BehaviourReturn - always success
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

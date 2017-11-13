using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class SequenceBehaviour
* child class of Behaviour
* 
* compound behaviour tree node that keeps executing
* child nodes unitl one returns FAILURE
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class SequenceBehaviour : Behaviour
{
    //list of children to execute
    public List<Behaviour> children = new List<Behaviour>();

    //position of node that returned pending, used to resume the node
    private int pendingI = 0;

    /*
    * Execute 
    * overrides Behaviour's Update()
    * 
    * calls upon children to run until one returns FAILURE in which case it also returns FAILURE
    * returns SUCCESS if no nodes return FAILURE
    * 
    * @returns BehaviourReturn - depends on what the condition node is checking for
    */
    public override BehaviourReturn Execute()
    {
        if (children.Count > 0)
        {
            //iterate through children or the latest child at the last iteration

            //get the size of the children list
            int childSize = children.Count;

            //iterate through all children, running the logic of the selector
            for (int i = pendingI; i < childSize; i++)
            {
                //store the child in a temp value
                Behaviour child = children[i];
                BehaviourReturn val = child.Execute();

                //pause the compound node and return PENDING
                if (val == BehaviourReturn.PENDING)
                {
                    pendingI = i;
                    return BehaviourReturn.PENDING;
                }
                else if (val == BehaviourReturn.FAILURE)
                {
                    pendingI = 0;

                    //a node returned FAILURE, stop and return FAILURE
                    return BehaviourReturn.FAILURE;
                }
            }

            //no nodes returned FAILURE, stop and return SUCCESS
            pendingI = 0;
            return BehaviourReturn.SUCCESS;
        }
        else
        {
            //the compound node was improperly initialised
            return BehaviourReturn.NONE;
        }
    }
}

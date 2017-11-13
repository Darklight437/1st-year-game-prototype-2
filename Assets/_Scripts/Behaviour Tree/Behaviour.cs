using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class Behaviour
* abstract class
* 
* base class that makes up all components of a behaviour tree
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public abstract class Behaviour : MonoBehaviour
{
    //custom type for returns on behaviour nodes
    public enum BehaviourReturn
    {
        NONE,
        SUCCESS,
        FAILURE,
        PENDING,
    }

    //reference to the tree that contains this node
    public BehaviourTree tree = null;

    /*
    * Execute 
    * abstract function
    * 
    * runs when the behaviour tree calls upon it in the update
    * 
    * @returns BehaviourReturn - the response from the node
    */
    public abstract BehaviourReturn Execute();

}

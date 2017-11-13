using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{

    //reference to the input of the tree
    public BaseInput input = null;

    //reference to the root of the tree, always the first node to be executed
    public Behaviour root = null;

    //private return value used by unity events
    public Behaviour.BehaviourReturn privateReturnValue = Behaviour.BehaviourReturn.NONE;

    //list of behaviour associated with the tree
    public List<Behaviour> behaviours = new List<Behaviour>();

	// Use this for initialization
	void Start ()
    {
        LinkNodes();
	}
	
	// Update is called once per frame
	void Update ()
    {
        root.Execute();
	}

    /*
    * LinkNodes 
    * 
    * supplies a reference to the behaviours to the tree
    * 
    * @returns void
    */
    public void LinkNodes()
    {
        //get the size of the behaviours list
        int behaviourSize = behaviours.Count;

        //iterate through all behaviours, setting the reference
        for (int i = 0; i < behaviourSize; i++)
        {
            behaviours[i].tree = this;
        }
    }
}

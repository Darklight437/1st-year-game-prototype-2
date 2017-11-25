using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class Item
* 
* base class for a menu item, triggers animations and automatically links them
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class Item : MonoBehaviour
{

    //two seperate lists depending on the edge type
    [HideInInspector]
    public List<BaseAnimation> entries = new List<BaseAnimation>();
    [HideInInspector]
    public List<BaseAnimation> exits = new List<BaseAnimation>();


    // Use this for initialization
    public void Initialise()
    {
        //get all of the animations on the item
        List<BaseAnimation> animations = new List<BaseAnimation>(GetComponentsInParent<BaseAnimation>());

        //get the size of the animations list
        int animationCount = animations.Count;

        //iterate through all animations, seperating them into two different lists depending on edge type
        for (int i = 0; i < animationCount; i++)
        {
            //store in a temp value
            BaseAnimation ba = animations[i];

            //determine what type of edge type the animation has
            if (ba.edgeType == BaseAnimation.EdgeType.ENTRY)
            {
                entries.Add(ba);
            }
            else if (ba.edgeType == BaseAnimation.EdgeType.EXIT)
            {
                exits.Add(ba);
            }

        }
    }


    /*
    * IsAnimating 
    * 
    * checks if there are still animations in the item running 
    * 
    * @returns bool - indicating if the item is still executing animations
    */
    public bool IsAnimating()
    {
        //get all of the animations on the item
        List<BaseAnimation> animations = new List<BaseAnimation>(entries);
        animations.AddRange(exits);

        //get the size of the animations list
        int animationCount = animations.Count;

        //iterate through all animations, seperating them into two different lists depending on edge type
        for (int i = 0; i < animationCount; i++)
        {
            //store in a temp value
            BaseAnimation ba = animations[i];

            //check if the timer has passed the maximum value for the animation
            if (ba.lerpValue < 1.0f)
            {
                return true;
            }
        }

        return false;
    }


    /*
    * Loop 
    * 
    * called once per frame while active
    * 
    * @param BaseAnimation.EdgeType edgeType - the type of animation to use
    * @returns void
    */
    public void Loop(BaseAnimation.EdgeType edgeType)
    {
        //determine what type of edge type to use in the loop function of the respective animations
        if (edgeType == BaseAnimation.EdgeType.ENTRY)
        {
            //turn on the game-object if it is off
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            //get the size of the relevant animations list
            int animationCount = entries.Count;

            //iterate through all of the animations, looping each
            for (int i = 0; i < animationCount; i++)
            {
                entries[i].Loop();
            }
        }
        else if (edgeType == BaseAnimation.EdgeType.EXIT)
        {
            //get the size of the relevant animations list
            int animationCount = exits.Count;

            //iterate through all of the animations, looping each
            for (int i = 0; i < animationCount; i++)
            {
                exits[i].Loop();
            }

            //if the item has stopped and is still active
            if (gameObject.activeSelf && !IsAnimating())
            {
                gameObject.SetActive(false);
            }
        }
    }
}

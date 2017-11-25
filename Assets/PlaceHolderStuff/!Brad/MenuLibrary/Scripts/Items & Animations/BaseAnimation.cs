using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class BaseAnimation
* 
* base class for an animation that plays on an item
* controls it's duration, time manipulation and delay
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class BaseAnimation : MonoBehaviour
{

    //type describing that modifies child animation's 
    public enum EdgeType
    {
        NONE,
        ENTRY,
        EXIT,
    }

    //animation parameters
    public EdgeType edgeType = EdgeType.ENTRY;
    public float timeDuration = 0.0f;
    public float timeDelay = 0.0f;
    public AnimationCurve timeCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

    //[HideInInspector]
    public float baseTimer = 0.0f;

    //needs to be public for access but doesn't need to be in the inspector
    public float lerpValue = 1.0f;


    /*
    * Loop 
    * virtual function
    * 
    * called once per frame while active
    * 
    * @returns void
    */
    public virtual void Loop()
    {
        baseTimer += Time.deltaTime;

        lerpValue = 0.0f;

        //check that the base timer has passed the starting point
        if (baseTimer > timeDelay)
        {
            //between 0 and 1
            lerpValue = timeCurve.Evaluate((baseTimer - timeDelay) / timeDuration);
        }

        //check that the base time hasn't passed the ending point
        if (baseTimer > timeDelay + timeDuration)
        { 
            lerpValue = 1.0f;
        }
    }
}

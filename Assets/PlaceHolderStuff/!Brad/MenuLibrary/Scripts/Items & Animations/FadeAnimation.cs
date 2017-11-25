using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class FadeAnimation
* child class of BaseAnimation
* 
* tunes the alpha channel over time
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class FadeAnimation : BaseAnimation
{

    //automated reference to the renderer of the sprites
    private List<CanvasRenderer> crs = null;

    /*
    * Loop 
    * overrides BaseAnimation's Loop()
    * 
    * called once per frame while active
    * 
    * @returns void
    */
    public override void Loop()
    {
        base.Loop();

        if (crs == null)
        {
            crs = new List<CanvasRenderer>(GetComponentsInChildren<CanvasRenderer>());
        }

        if (edgeType == EdgeType.ENTRY)
        {
            int crCount = crs.Count;

            for (int i = 0; i < crCount; i++)
            {
                crs[i].SetAlpha(lerpValue);
            }
        }
        else if (edgeType == EdgeType.EXIT)
        {
            int crCount = crs.Count;

            for (int i = 0; i < crCount; i++)
            {
                crs[i].SetAlpha(1 - lerpValue);
            }
        }
        
    }
}
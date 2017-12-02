using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
* class SpriteRender
* inherits MonoBehaviour
* 
* simple little script to swap out sprits every 24th of a second to an image
* commponent it is connected to
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
public class SpriteRender : MonoBehaviour
{
    //array holdinga ll the sprites needed for the basic sprit animation for team colours
    public Sprite[] blueTeam;
    public Sprite[] redTeam;

    //this is used to determin whos turn it currently is
    public bool isBlueTeam = true;

    //refrence to the image that we will be passing the sprites into
    private Image m_image;

    //this is the array num for what position we are up to in the array showing our sprites
    private int m_count = 0;
    //this is the timer used to determin when we should swap out our next sprite
    private float m_timer;

    public void Start()
    {
        //get the image component
        m_image = GetComponent<Image>();
    }

    void Update ()
    {
        //add time to the timer
        m_timer += Time.deltaTime;

        //this should make it so that it swaps out the images once every 24th of a second
        if (m_timer >= 0.0416)
        {
            //loop count back around if its hit the limit
            if (m_count >= 100)
            {
                m_count = 0;
            }

            //make sure the count is not outside any of the arrays bounds
            if (m_count >= 0 && m_count < blueTeam.Length && m_count < redTeam.Length)
            {
                //if we are on blue team swap out image with next blue team else do that for red
                if (isBlueTeam)
                {
                    m_image.sprite = blueTeam[m_count];
                }
                else
                {
                    m_image.sprite = redTeam[m_count];
                }
            }
            else
            {
                //reset count if we where out of boounds
                m_count = 0;
            }

            //increase the count after we have done all the things
            m_count++;
        }
		
	}
}

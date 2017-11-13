using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowTest : MonoBehaviour {

    public GameObject testModel = null;
    //private Color clear = Color.
     public bool UnitActive = false;
    // Use this for initialization

    Shader DefaultShader;
    Shader WallThrough;

    SkinnedMeshRenderer TheRenderer;

    void Start ()
    {
        TheRenderer = testModel.GetComponentInChildren<SkinnedMeshRenderer>();

        DefaultShader = Shader.Find("Custom/DefaultShader");
        WallThrough = Shader.Find("Custom/DefaultShader");

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    public void GlowLikeABastard()
    {
        if(UnitActive)
        {
            TheRenderer.material.shader = Shader.Find("Custom/DefaultShader");
        }
        else
        {
            TheRenderer.material.shader = Shader.Find("Custom/WallThrough");
        }
        UnitActive = !UnitActive;



    }
}

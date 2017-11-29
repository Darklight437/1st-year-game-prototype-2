using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Winscreen : MonoBehaviour {
    public Material mat;
    Renderer DISPLAY;
    public List<Texture> FrameList;
    public float playBackSpeed = 0.032f;
    float Timer;
    int currentFrame;
    public bool isLooping = false;
    bool isPlaying = true;
    float Depth = 5.0f;

    GameObject PlaneQuad;

    // Use this for initialization
    void Start () {
        //Creates a Quad that is the same aspect ratio to the camera locked.
        currentFrame = 0;
        
        PlaneQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);

        float Size = (((Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Depth)) -
            Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Depth))).magnitude));

        PlaneQuad.transform.parent = Camera.main.transform;
        PlaneQuad.transform.localPosition = new Vector3(0, 0, Depth);
        PlaneQuad.transform.forward = Camera.main.transform.forward;
        PlaneQuad.transform.localScale = new Vector3((Size * Camera.main.aspect), Size, 1);
        PlaneQuad.transform.localRotation = Quaternion.identity;

        DISPLAY = PlaneQuad.GetComponent<Renderer>();
        DISPLAY.material = mat;

        DISPLAY.material.mainTexture = FrameList[currentFrame];

    }
	
	// Update is called once per frame
	void Update () {
        if(isPlaying)
        {
            Timer += Time.deltaTime;
            if (Timer > playBackSpeed)
            {
                Timer = 0;
                currentFrame++;
                if (currentFrame >= FrameList.Count)
                {
                    if (isLooping)
                    {
                        currentFrame = 0;
                    }
                    else
                    {
                        isPlaying = false;
                    }
                }
                DISPLAY.material.mainTexture = FrameList[currentFrame];
            }
        }	
	}
}

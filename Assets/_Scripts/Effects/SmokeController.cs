using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    public ParticleSystem Smoke;


     void Awake()
    {
        TurnSmokeOff();
    }

    public void TurnSmokeOn()
    {
        Smoke.Play();
    }


    public void TurnSmokeOff()
    {
        Smoke.Stop();
    }


}
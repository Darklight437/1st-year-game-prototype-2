using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    public ParticleSystem Smoke;
    public ParticleSystem Smoke2;


     void Awake()
    {
        TurnSmokeOff();
    }

    public void TurnSmokeOn()
    {
        Smoke.Play();
        Smoke2.Play();
    }


    public void TurnSmokeOff()
    {
        Smoke.Stop();
        Smoke2.Stop();
    }


}
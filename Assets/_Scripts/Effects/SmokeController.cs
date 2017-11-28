using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    public ParticleSystem Smoke;
    public ParticleSystem Smoke2;
    public ParticleSystem CustomParticle;

     void Awake()
    {
        TurnSmokeOff();
    }

    public void TurnSmokeOn()
    {
        if (Smoke)
        {
            Smoke.Play();
        }
        if (Smoke2)
        {
            Smoke2.Play();
        }
        
        
    }


    public void TurnSmokeOff()
    {
        if (Smoke)
        {
            Smoke.Stop();
        }
        if (Smoke2)
        {
            Smoke2.Stop();
        }
        
    }
    //for custom particles like melee punch
    public void turnCustomParticleOn()
    {
        if (CustomParticle)
        {
            CustomParticle.Play();
        }
    }

    public void turnCustomParticleOff()
    {
        if (CustomParticle)
        {
            CustomParticle.Stop();
        }
    }


}
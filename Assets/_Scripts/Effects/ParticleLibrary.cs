using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class ParticleLibrary
*
* singleton containing all particle systems in the game
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public static class ParticleLibrary
{

    //particle system references
    public static ParticleSystem explosionSystem = null;

    /*
    * GetSystems
    * 
    * searches for all of the particle systems in the game
    * and assigns them in the memory of this singleton
    * 
    * @returns void
    */
    public static void GetSystems()
    {
        //get array of all particle systems in the scene
        ParticleSystem[] systems = GameObject.FindObjectsOfType<ParticleSystem>();

        //get the size of the systems array
        int sysCount = systems.GetLength(0);

        //iterate through all particle systems, identifying each
        for (int i = 0; i < sysCount; i++)
        {
            //store in a temp variable
            ParticleSystem system = systems[i];

            switch(system.name)
            {
                case "Explosion": explosionSystem = system; break;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    //refrence to the unit this sight belongs to
    public Unit myUnit;
    
    public void OnTriggerStay(Collider other)
    {
        Unit U = other.GetComponent<Unit>();
        if (U != null)
        {
            U.TurnOnRender();           
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Unit U = other.GetComponent<Unit>();
        if (U != null)
        {
            U.TurnOffRender();

        }
    }
}

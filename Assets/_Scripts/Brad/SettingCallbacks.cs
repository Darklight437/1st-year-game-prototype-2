using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingCallbacks : MonoBehaviour
{

    //two callbacks one, the first sets the name of the variable, the second passes in the value
    //this is done because unity's event system doesn't allow for events with multiple parameters
    private string m_variableTarget = "";

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    /*
    * SetTarget
    * 
    * sets internal variable m_variableTarget to the input
    * 
    * @param string target - the name of the variable to target
    * @returns void 
    */
    public void SetTarget(string target)
    {
        m_variableTarget = target;
    }


    /*
    * OnBoolChanged
    * 
    * callback for when a bool value is changed
    * 
    * @param bool value - the new value for variable of "m_variableTarget" 
    * @returns void
    */
    public void OnBoolChanged(bool value)
    {
        Settings.SetValue<bool>(m_variableTarget, value);
    }


    /*
    * OnFloatChanged
    * 
    * callback for when a float value is changed
    * 
    * @param float value - the new value for variable of "m_variableTarget" 
    * @returns void
    */
    public void OnFloatChanged(float value)
    {
        Settings.SetValue<float>(m_variableTarget, value);
    }


}

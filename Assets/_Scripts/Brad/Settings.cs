using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/*
* class Settings
*
* singleton that contains game parameters eg. AI playing?, volume, difficulty
*
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public static class Settings
{
    //ai parameters
    public static bool playingWithAI = false;
    public static float aiDifficulty = 0.5f;

    //general parameters
    public static float volume = 0.5f;
    public static float mouseSensitivity = 0.5f;
    public static float brightness = 0.5f;


    /*
    * GetValue<T>
    * template function
    * 
    * searches for a variable with the given name and returns it's value
    * 
    * @param string variableName - the name of the variable in the class
    * @returns T - the value of the variable
    */
    public static T GetValue<T>(string variableName)
    {
        //get the type to access information about the singleton's contents
        System.Type type = typeof(Settings);

        //get information about the field
        FieldInfo info = type.GetField(variableName);

        //cast to the given type and return the value
        return (T)info.GetValue(null);

    }


    /*
    * SetValue<T> 
    * template function
    * 
    * searches for a variable with the given name and alters it's value
    * the function will fail silently if the variable doesn't exist
    * 
    * @param string variableName - the name of the variable in the class
    * @param T value - the new value to give the variable
    * @returns void
    */
    public static void SetValue<T>(string variableName, T value)
    {
        //get the type to access information about the singleton's contents
        System.Type type = typeof(Settings);

        //get the variable from the type object
        FieldInfo field = type.GetField(variableName);

        //set the value (pass in null to get the singleton's values)
        field.SetValue(null, value);

    }
}

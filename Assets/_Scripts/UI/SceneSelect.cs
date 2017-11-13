using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour {
    //David

    //public enum that countains each scene by build index
    public enum eChangeSceneTo { MENU, PLAY };
    public eChangeSceneTo NextScene;


    /*
     * SceneChange
     * 
     * called whenever a button that changes the scene is pressed 
     * 
     * @returns void
     */
    public void SceneChange()
    {
        if (SceneManager.GetActiveScene().buildIndex != (int)NextScene)
        {
            SceneManager.LoadScene(((int)NextScene));
        }
    }
}

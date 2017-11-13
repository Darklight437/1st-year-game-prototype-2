#if (UNITY_EDITOR) 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
* class MapEditor
* inherits Editor
* 
* this changes all the inspector stuff for the map editor
* it adds in the buttons to generate the map and slecte what tiles to paint
* and it also overrides unitys mouse functions for when you are in paint mode
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
[CustomEditor(typeof(MapEditorScript))]
public class MapEditor : Editor
{

    /*
    * OnInspectorGUI
    * virtual function override
    * 
    * this overrides the default inspector for or MapEditorScript allowing
    * for our own custome buttons to be displayed there
    * 
    * @returns nothing
    */
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapEditorScript myScript = (MapEditorScript)target;

        //adds in a button to generate a default map
        //of normal tiles when clicked
        if (GUILayout.Button("Generate base Map"))
        {
            myScript.GenerateBasMap();
        }

        //this button will call a function to make a simple 5,5 chunk of tiles
        if (GUILayout.Button("Generate Map Chunk"))
        {
            myScript.GenerateMapChunk();
        }

        //this button calls a function in MapEditorScript to toggle on and off paint mode
        if (GUILayout.Button("Toggle paint mode"))
        {
            myScript.TogglePaint();
        }

        //this will go through a map and update its tiles with the latest tile prefab
        if (GUILayout.Button("Update Tile Prefab"))
        {
            myScript.UpdateTilePrefabs();
        }

        //these buttons will only display if paint mode is on
        if (myScript.enablePaint)
        {
            //this paints all the tiles as one specific tile on the chossen map
            if (GUILayout.Button("Paint All"))
            {
                myScript.PaintAll();
            }

            //allows you to paint normal tiles on a map
            if (GUILayout.Button("Slect Normal"))
            {
                myScript.SetPaintTypeNormalTile();
            }

            //allows you to paint damage tiles on a map
            if (GUILayout.Button("Slect Damage"))
            {
                myScript.SetPaintTypeDamageTile();
            }

            //allows you to paint Defense tiles on a map
            if (GUILayout.Button("Slect Defense"))
            {
                myScript.SetPaintTypeDefenseTile();
            }

            //allows you to paint impassible tiles on a map
            if (GUILayout.Button("Slect Impassable"))
            {
                myScript.SetPaintTypeImpassibleTile();
            }
        }
    }

    /*
    * OnSceneGUI
    * 
    * this function is where we override unitys mouse controls 
    * to implament our own ones when in paint mode
    * 
    * @returns nothing
    */
    void OnSceneGUI()
    {
        MapEditorScript myScript = (MapEditorScript)target;

        //gets the event stuff needed to override the mouse
        Event e = Event.current;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);

        //only overrides when enable paint is true
        if (myScript.enablePaint)
        {
            //switch statements for the override
            switch (e.GetTypeForControl(controlID))
            {
                //when we click down the paint
                case EventType.MouseDown:
                    GUIUtility.hotControl = controlID;
                    myScript.Paint();
                    e.Use();
                    break;

                case EventType.MouseUp:
                    GUIUtility.hotControl = 0;
                    e.Use();
                    break;

                    //when we drag we also paint
                case EventType.MouseDrag:
                    GUIUtility.hotControl = controlID;
                    myScript.Paint();
                    e.Use();
                    break;

                case EventType.KeyDown:
                    break;
            }
        }
    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskedSpriteList : MonoBehaviour
{
    //type for display formats of the values
    public enum DisplayType
    {
        INT,
        FLOAT,
        TIME,
    }

    //get a reference to the display font
    public Font displayFont;

    //the position of the list in the scrollable space
    public float scrollValue = 0.0f;

    //how big the text is
    public int fontSize = 25;

    //where the first text entry starts, and the distance to the next
    public float pivotOffset = 0.0f;
    public float pivotStart = 100.0f;

    [HideInInspector]
    //list of text objects
    public List<Text> pivots;

    //list of strings and floats containing the display names of variables and their values
    public List<string> names;
    public List<float> values;
    public List<DisplayType> displayTypes;

    // Use this for initialization
    void Start()
    {
        pivots = new List<Text>();

        GenerateColumn();
    }


    /*
    * GenerateColumn
    * 
    * tells the masked list to instantiate the text objects
    * and give them the desired values for display
    * 
    * @returns void
    */
    public void GenerateColumn()
    {
        //get the size of the values list
        int valueSize = values.Count;

        //iterate through all of the values, creating a pivot text object for each
        for (int i = 0; i < valueSize; i++)
        {
            //create a new gameobject
            GameObject obj = new GameObject();

            //set the name of the object
            obj.name = names[i];

            //set the parent of the new gameobject
            obj.transform.parent = transform;

            //centre the object
            obj.transform.localPosition = Vector3.zero;

            //add a new text component to the object
            Text text = obj.AddComponent<Text>();

            //determing the correct formatting
            switch (displayTypes[i])
            {
                case DisplayType.INT: text.text = names[i] + ": " + ((int)values[i]).ToString(); break;
                case DisplayType.FLOAT: text.text = names[i] + ": " + values[i].ToString(); break;
                case DisplayType.TIME:

                    int hours = Mathf.FloorToInt(values[i]);
                    int minutes = Mathf.FloorToInt((values[i] - hours) * 60.0f);

                    text.text = names[i] + ": " + hours.ToString() + "h " + minutes.ToString() + "m"; break;
            }


            text.transform.localPosition = new Vector3(0.0f, pivotStart - pivotOffset * i + scrollValue, 0.0f);

            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.font = displayFont;
            text.fontSize = fontSize;
            text.alignment = TextAnchor.MiddleCenter;

            pivots.Add(text);
        }
    }


    /*
    * UpdateColumn
    * 
    * updates all of the masked list's values
    * 
    * @returns void
    */
    public void UpdateColumn()
    {
        //get the size of the values list
        int valueSize = pivots.Count;

        //iterate through all of the values, creating a pivot text object for each
        for (int i = 0; i < valueSize; i++)
        {
            //create a new gameobject
            GameObject obj = pivots[i].gameObject;

            //centre the object
            obj.transform.localPosition = Vector3.zero;

            //add a new text component to the object
            Text text = obj.GetComponent<Text>();

            //determing the correct formatting
            switch (displayTypes[i])
            {
                case DisplayType.INT: text.text = names[i] + ": " + ((int)values[i]).ToString(); break;
                case DisplayType.FLOAT: text.text = names[i] + ": " + values[i].ToString(); break;
                case DisplayType.TIME:

                    int hours = Mathf.FloorToInt(values[i]);
                    int minutes = Mathf.FloorToInt((values[i] - hours) * 60.0f);

                    text.text = names[i] + ": " + hours.ToString() + "h " + minutes.ToString() + "m"; break;
            }


            text.transform.localPosition = new Vector3(0.0f, pivotStart - pivotOffset * i + scrollValue, 0.0f);

            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.font = displayFont;
            text.fontSize = fontSize;
            text.alignment = TextAnchor.MiddleCenter;
        }
    }

    private Vector2 previousMouse = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        UpdateColumn();

        Vector2 mouse = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            float yScaleIncrease = 1 / transform.lossyScale.y;

            //get the change in position from the last frame
            Vector2 delta = previousMouse - mouse;

            //change the scroll value, but consider the scale
            scrollValue -= delta.y * yScaleIncrease;

            //clamp the scroll value to 0.0 and the maximum possible value
            scrollValue = Mathf.Clamp(scrollValue, 0.0f, (pivotOffset * pivots.Count));
        }

        previousMouse = mouse;
    }
}

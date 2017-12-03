using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/*
* class AchievementsTracker
* child class of monobehaviour
*
* singleton (uses a large amount of static variables)/behaviour 
* contains real-time game achievements
* displays them when achieved and saves them to a file
*
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class AchievementsTracker : MonoBehaviour {

    //set the display of the statistics tracker
    public MaskedStringList display = null;

    private static bool m_firstStart = true;

    public void Start()
    {
        AchievementsTracker.Initialise(display);
    }


    public void OnApplicationQuit()
    {
        WriteData();
    }

    //achievement flags
    public static bool outOfTheGates = false;
    public static bool firstBlood = false;
    public static bool ohBaby = false;
    public static bool bigDaddy = false;
    public static bool highMotor = false;
    public static bool digitalMassacre = false;
    public static bool efficientDoctor = false;
    public static bool notThatBad = false;


    /*
    * Initialise
    * 
    * reads all of the saved data from the achievements file (or makes the file)
    * and initialises the passed in masked list
    * 
    * @param MaskedStringList - the display object for the achievements
    * @returns void
    */
    public static void Initialise(MaskedStringList list)
    {
        string path = Application.dataPath + "/achivements.bin";

        //don't load data if it has already been loaded
        if (AchievementsTracker.m_firstStart)
        {
            //if the file exists, load it else make a new one
            if (File.Exists(path))
            {

                //open a file stream to the stats.dat file
                FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);

                //get the binary reader for the file
                BinaryReader br = new BinaryReader(file);

                //read all of the variables form the file
                outOfTheGates = bool.Parse(br.ReadString());
                br.ReadString();
                firstBlood = bool.Parse(br.ReadString());
                br.ReadString();
                ohBaby = bool.Parse(br.ReadString());
                br.ReadString();
                bigDaddy = bool.Parse(br.ReadString());
                br.ReadString();
                highMotor = bool.Parse(br.ReadString());
                br.ReadString();
                bigDaddy = bool.Parse(br.ReadString());
                br.ReadString();
                digitalMassacre = bool.Parse(br.ReadString());
                br.ReadString();
                efficientDoctor = bool.Parse(br.ReadString());
                br.ReadString();
                notThatBad = bool.Parse(br.ReadString());
                br.ReadString();

                file.Close();
            }
            else
            {
                WriteData();
            }

            AchievementsTracker.m_firstStart = false;
        }


        //display name of the item                                                              value to give the item                                   display type of the item (whole number, decimal, time stamp)
        list.names.Add("Out of the Gates - Play One Game");                                     list.values.Add(ConvertBoolToFloat(outOfTheGates));      list.displayTypes.Add(MaskedStringList.DisplayType.ENABLE);
        list.names.Add("First Blood - Kill One Enemy");                                         list.values.Add(ConvertBoolToFloat(firstBlood));         list.displayTypes.Add(MaskedStringList.DisplayType.ENABLE);
        list.names.Add("Oh Baby - Kill Three Enemies in One Turn");                             list.values.Add(ConvertBoolToFloat(ohBaby));             list.displayTypes.Add(MaskedStringList.DisplayType.ENABLE);
        list.names.Add("BigDaddy- Invoke the BIGDADDY Protocool");                              list.values.Add(ConvertBoolToFloat(bigDaddy));           list.displayTypes.Add(MaskedStringList.DisplayType.ENABLE);
        list.names.Add("Digital Massacre - defeat an AI of the highest difficulty");            list.values.Add(ConvertBoolToFloat(digitalMassacre));    list.displayTypes.Add(MaskedStringList.DisplayType.ENABLE);
        list.names.Add("Efficient Doctor - Heal more than 400 health per turn on average");     list.values.Add(ConvertBoolToFloat(efficientDoctor));    list.displayTypes.Add(MaskedStringList.DisplayType.ENABLE);
        list.names.Add("Not That Bad - Play for an hour");                                      list.values.Add(ConvertBoolToFloat(notThatBad));         list.displayTypes.Add(MaskedStringList.DisplayType.ENABLE);
    }


    /*
    * WriteData 
    * 
    * writes all of the data to the stats file
    * 
    * @returns void
    */
    public static void WriteData()
    {
        string path = Application.dataPath + "/achievements.bin";

        //open a file stream to the stats.dat file
        FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);

        //get the binary writer for the file
        BinaryWriter bw = new BinaryWriter(file);

        //write all of the default values to a file
        bw.Write(outOfTheGates.ToString());
        bw.Write(",");
        bw.Write(firstBlood.ToString());
        bw.Write(",");
        bw.Write(ohBaby.ToString());
        bw.Write(",");
        bw.Write(bigDaddy.ToString());
        bw.Write(",");
        bw.Write(digitalMassacre.ToString());
        bw.Write(",");
        bw.Write(efficientDoctor.ToString());
        bw.Write(",");
        bw.Write(notThatBad.ToString());

        file.Close();
    }


    /*
    * CheckAchievements
    * 
    * checks if any of the requirements for achievements have been met
    * 
    * @returns void
    */
    public static void CheckAchievements()
    {
        if (!outOfTheGates && StatisticsTracker.gamesPlayed > 0)
        {
            outOfTheGates = true;
        }

        if (!firstBlood && StatisticsTracker.unitsDefeated > 0)
        {
            firstBlood = true;
        }

        if (!ohBaby && StatisticsTracker.mostUnitsDefeatedInASingleTurn >= 3)
        {
            ohBaby = true;
        }

        if (!bigDaddy)
        {
            //ohBaby = true;
        }

        if (!digitalMassacre)
        {
            //ohBaby = true;
        }

        if (!efficientDoctor && StatisticsTracker.healingPerTurn >= 450)
        {
            efficientDoctor = true;
        }

        if (!notThatBad && StatisticsTracker.timeSpentPlaying >= 1.0f)
        {
            notThatBad = true;
        }
    }


    /*
    * ConvertBoolToFloat
    * 
    * converts a bool to a float (0.0f for false, 1.0f for true)
    * 
    * @param bool value - the bool to convert
    * @returns float - the converted bool
    */
    public static float ConvertBoolToFloat(bool value)
    {
        return value ? 1.0f : 0.0f;
    }
}

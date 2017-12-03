using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/*
* class StatisticsTracker
* child class of monobehaviour
*
* singleton (uses a large amount of static variables) /behaviour that contains real-time game statistics and saves them to a file
*
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class StatisticsTracker : MonoBehaviour
{
    //set the display of the statistics tracker
    public MaskedStringList display = null;

    private static bool m_firstStart = true;

    public void Start()
    {
        StatisticsTracker.Initialise(display);
    }


    public void OnApplicationQuit()
    {
        WriteData();    
    }


    //statistical variables
    public static int gamesPlayed = 0;
    public static int unitsDefeated = 0;
    public static int turnsPlayed = 0;
    public static float turnsPerGame = 0;
    public static float healingPerTurn = 0.0f;
    public static float damagePerTurn = 0.0f;
    public static float timeSpentPlaying = 0.0f;
    public static float totalHealing = 0.0f;
    public static float totalDamage = 0.0f;
    public static int mostUnitsDamagedWithOneAttack = 0;
    public static int mostUnitsDefeatedInASingleTurn = 0;
    public static int tilesCrossed = 0;


    /*
    * Initialise
    * 
    * reads all of the saved data from the stats file (or makes the file)
    * and initialises the passed in masked list
    * 
    * @param MaskedStringList - the display object for the statistics
    * @returns void
    */
    public static void Initialise(MaskedStringList list)
    {
        string path = Application.dataPath + "/stats.bin";

        //don't load data if it has already been loaded
        if (StatisticsTracker.m_firstStart)
        {
            //if the file exists, load it else make a new one
            if (File.Exists(path))
            {

                //open a file stream to the stats.dat file
                FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);

                //get the binary reader for the file
                BinaryReader br = new BinaryReader(file);

                //read all of the variables form the file
                gamesPlayed = int.Parse(br.ReadString());
                br.ReadString();
                unitsDefeated = int.Parse(br.ReadString());
                br.ReadString();
                gamesPlayed = int.Parse(br.ReadString());
                br.ReadString();
                turnsPlayed = int.Parse(br.ReadString());
                br.ReadString();
                turnsPerGame = float.Parse(br.ReadString());
                br.ReadString();
                healingPerTurn = float.Parse(br.ReadString());
                br.ReadString();
                damagePerTurn = float.Parse(br.ReadString());
                br.ReadString();
                timeSpentPlaying = float.Parse(br.ReadString());
                br.ReadString();
                totalHealing = float.Parse(br.ReadString());
                br.ReadString();
                totalDamage = float.Parse(br.ReadString());
                br.ReadString();
                mostUnitsDamagedWithOneAttack = int.Parse(br.ReadString());
                br.ReadString();
                mostUnitsDefeatedInASingleTurn = int.Parse(br.ReadString());
                br.ReadString();
                tilesCrossed = int.Parse(br.ReadString());

                file.Close();
            }
            else
            {
                WriteData();
            }

            StatisticsTracker.m_firstStart = false;
        }

        CalculateAverages();

        //display name of the item                                  value to give the item                              display type of the item (whole number, decimal, time stamp)
        list.names.Add("Games Played");                             list.values.Add(gamesPlayed);                       list.displayTypes.Add(MaskedStringList.DisplayType.INT);
        list.names.Add("Units Defeated");                           list.values.Add(unitsDefeated);                     list.displayTypes.Add(MaskedStringList.DisplayType.INT);
        list.names.Add("Turns Played");                             list.values.Add(turnsPlayed);                       list.displayTypes.Add(MaskedStringList.DisplayType.INT);
        list.names.Add("Turns Per Game");                           list.values.Add(turnsPerGame);                      list.displayTypes.Add(MaskedStringList.DisplayType.FLOAT);
        list.names.Add("Healing Per Turn");                         list.values.Add(healingPerTurn);                    list.displayTypes.Add(MaskedStringList.DisplayType.FLOAT);
        list.names.Add("Damage Per Turn");                          list.values.Add(damagePerTurn);                     list.displayTypes.Add(MaskedStringList.DisplayType.FLOAT);
        list.names.Add("Time Spent Playing");                       list.values.Add(timeSpentPlaying);                  list.displayTypes.Add(MaskedStringList.DisplayType.TIME);
        list.names.Add("Total Healing");                            list.values.Add(totalHealing);                      list.displayTypes.Add(MaskedStringList.DisplayType.FLOAT);
        list.names.Add("Total Damage");                             list.values.Add(totalDamage);                       list.displayTypes.Add(MaskedStringList.DisplayType.FLOAT);
        list.names.Add("Most Units Damaged in a Single Attack");    list.values.Add(mostUnitsDamagedWithOneAttack);     list.displayTypes.Add(MaskedStringList.DisplayType.INT);
        list.names.Add("Most Units Defeated in a Single Turn");     list.values.Add(mostUnitsDefeatedInASingleTurn);    list.displayTypes.Add(MaskedStringList.DisplayType.INT);
        list.names.Add("Tiles Crossed");                            list.values.Add(tilesCrossed);                      list.displayTypes.Add(MaskedStringList.DisplayType.INT);
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
        string path = Application.dataPath + "/stats.bin";

        //open a file stream to the stats.dat file
        FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);

        //get the binary writer for the file
        BinaryWriter bw = new BinaryWriter(file);

        //write all of the default values to a file
        bw.Write(gamesPlayed.ToString());
        bw.Write(",");
        bw.Write(unitsDefeated.ToString());
        bw.Write(",");
        bw.Write(gamesPlayed.ToString());
        bw.Write(",");
        bw.Write(turnsPlayed.ToString());
        bw.Write(",");
        bw.Write(turnsPerGame.ToString());
        bw.Write(",");
        bw.Write(healingPerTurn.ToString());
        bw.Write(",");
        bw.Write(damagePerTurn.ToString());
        bw.Write(",");
        bw.Write(timeSpentPlaying.ToString());
        bw.Write(",");
        bw.Write(totalHealing.ToString());
        bw.Write(",");
        bw.Write(totalDamage.ToString());
        bw.Write(",");
        bw.Write(mostUnitsDamagedWithOneAttack.ToString());
        bw.Write(",");
        bw.Write(mostUnitsDefeatedInASingleTurn.ToString());
        bw.Write(",");
        bw.Write(tilesCrossed.ToString());

        file.Close();
    }


    /*
    * CalculateAverages 
    * 
    * takes the amount of games played and various total
    * statistics and calculates the averages for each game
    *  
    * @returns void
    */
    public static void CalculateAverages()
    {
        turnsPerGame = 0.0f;
        healingPerTurn = 0.0f;
        damagePerTurn = 0.0f;

        //averages won't work if the denominator is 0
        if (gamesPlayed > 0)
        {
            turnsPerGame = turnsPlayed / (float)gamesPlayed;
        }

        if (turnsPlayed > 0)
        {
            healingPerTurn = totalHealing / (float)turnsPlayed;
            damagePerTurn = totalDamage / (float)turnsPlayed;
        }
             
    }

}

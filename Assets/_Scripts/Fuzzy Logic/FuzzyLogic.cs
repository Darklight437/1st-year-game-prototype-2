using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyLogic : MonoBehaviour
{
    //input module for the fuzzy logic
    public BaseInput input = null;

    //functions that get scored depending on what has the highest score
    public List<FuzzyFunction> functions = new List<FuzzyFunction>();

    public void Execute()
    {
        //remember the best score and the associated function
        float bestScore = float.MinValue;
        FuzzyFunction bestFunc = null;

        //get the size of the functions list
        int funcSize = functions.Count;

        //iterate through all functions
        for (int i = 0; i < funcSize; i++)
        {
            //store in a temp value
            FuzzyFunction func = functions[i];

            //get the score
            float score = func.evaluation(input);

            //check if the score is higher than the current best score
            if (score > bestScore)
            {
                //set the new best score
                bestScore = score;
                bestFunc = func;
            }
        }

        //execute the best function (if it exists)
        if (bestFunc != null)
        {
            bestFunc.execution(input);
        }


    }
}

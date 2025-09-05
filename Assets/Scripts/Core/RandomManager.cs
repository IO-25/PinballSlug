using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomManager
{
    public static bool FlipCoin(float successRate)
    {
        float randomNumber = Random.Range(0.0f, 1.0f);
        return randomNumber <= successRate;
    }

    public static int PickOne(int endIndex)
    {
        float prob = 1.0f / (endIndex+1);
        float randomNumber = Random.Range(0.0f, 1.0f);
        int index = 0;
        while (true)
        {
            randomNumber -= prob;
            if (randomNumber <= 0.0f)
                return index;
            index++;
        }
    }

    public static int RandomPicker(float[] Probability)
    {
        float sum = 0;
        for (int i = 0; i < Probability.Length; i++)
        {
            sum += Probability[i];
        }
        if (sum == 0)
        {
            throw new System.Exception("Array of Probability sum of 0");
        }
        for (int i = 0; i < Probability.Length; i++)
        {
            Probability[i] /= sum;
        }

        float randomNumber = Random.Range(0.0f, 1.0f);
        for (int i = 0; i < Probability.Length; i++)
        {
            randomNumber -= Probability[i];
            if (randomNumber <= 0)
                return i;
        }
        throw new System.Exception("Probability Out Of Range");
    }

}

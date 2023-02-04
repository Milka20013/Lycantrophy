using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public static T GetRandomElementFromFairTable<T>(T[] table)
    {
        int rndIndex = Random.Range(0, table.Length);
        return table[rndIndex];
    }
    public static int[] CreateSequenceOfRange(int[] tailHead, int increment = 1)
    {
        if (tailHead[0] == tailHead[1])
        {
            return tailHead;
        }
        int count = Mathf.CeilToInt((tailHead[1] - tailHead[0]) / increment) + 1;
        int[] array = new int[count];
        for (int i = 0; i < count; i++)
        {
            array[i] = tailHead[0] + increment * i;
        }
        return array;
    }

    public static T GetRandomElementFromWeightedTable<T>(T[] table, float[] percentages)
    {
        float rndValue = Random.value;
        float nextValue = rndValue;
        int rndIndex = 0;
        float[] weigths = new float[percentages.Length];
        for (int i = 0; i < percentages.Length; i++)
        {
            weigths[i] = percentages[i] / 100;
        }
        for (int i = 0; i < weigths.Length; i++)
        {
            nextValue = nextValue - weigths[i];
            if (nextValue <= 0f)
            {
                rndIndex = i;
                break;
            }
        }
        return table[rndIndex];
    }
    public static int GetRandomElementByPercentage(float percentage, int min = 1, int max = 1, bool sequence = true, int increment = 1)
    {
        float rndValue = Random.value;
        int number = 0;
        int[] range;
        if (sequence)
        {
            range = CreateSequenceOfRange(new int[] { min, max }, increment);
        }
        else
        {
            range = new int[] { min, max };
        }
        if (rndValue < percentage / 100)
        {
            number = GetRandomElementFromFairTable(range);
            int modifier = Mathf.FloorToInt(percentage / 100);
            if (modifier > 0)
            {
                float remainder = percentage - modifier * 100;
                modifier += GetRandomElementFromWeightedTable(new int[] { 0, 1 }, new float[] { 100 - remainder, remainder });
                number = number * modifier;
            }

        }
        return number;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Duke of Disastr
public struct UsefulFunctions 
{
    public static float CutDecimal(float number)
    {
        float roundedNumber = Mathf.Round(number);
        if (roundedNumber > number)
        {
            float z = roundedNumber - number;
            float a = 1 - z;
            return number - a;
        }
        else
        {
            return roundedNumber;
        }
    }
    public static float CutDecimal(float number, out float decimalReturn)
    {
        float roundedNumber = Mathf.Round(number);

        if (roundedNumber > number)
        {
            float difference = roundedNumber - number;
            float decimalNumber = 1 - difference;
            decimalReturn = decimalNumber;
            return number - decimalNumber;
        }
        else
        {
            float difference = number - roundedNumber;
            decimalReturn = difference;
            return roundedNumber;
        }
    }
    public static void Teleport(Transform a, Transform b)
    {
        a.position = b.position;
    }

    public static bool ExclusiveOr(bool a, bool b)
    {
        return ((a && !b) || (!a && b));
    }
    public static T ReturnRandomElement<T>(T[] list)
    {
        return list[Mathf.RoundToInt(Random.Range(0, list.Length - 1))];
    }
}

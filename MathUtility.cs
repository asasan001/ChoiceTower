using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MathUtility 
{
    public static System.Random Rnd = new System.Random();
    public static (float Z1, float Z2) BoxMuller(float ave = 0f, float sigma =1f) {
        double X = Rnd.NextDouble();
        double Y = Rnd.NextDouble();
        float Z1 = (float)(sigma * Math.Sqrt(-2.0 * Math.Log(X)) * Math.Cos(2.0 * Math.PI * Y) + ave);
        float Z2 = (float)(sigma * Math.Sqrt(-2.0 * Math.Log(X)) * Math.Sin(2.0 * Math.PI * Y) + ave);
        return (Z1, Z2);
    }
}

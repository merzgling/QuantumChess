using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Geometry
{
    public static int dist(Field f1, Field f2)
    {
        return Mathf.Abs(f1.x - f2.x) + Mathf.Abs(f1.y - f2.y);
    }

    public static bool stayOnSameLine(Field f1, Field f2)
    {
        if (f1.x == f2.x || f1.y == f2.y)
            return true;
        else
            return false;
    }
}

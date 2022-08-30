using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Clamps a given vector to a maximum magnitude
    /// </summary>
    /// <param name="v">Vector to be clamped</param>
    /// <param name="maxMagnitude">Maximum magnitude of returned vector</param>
    public static Vector2 Clamp(this Vector2 v, float maxMagnitude)
    {
        if(v.SqrMagnitude() > (maxMagnitude * maxMagnitude))
        {
            return v.normalized * maxMagnitude;
        }
        return v;
    }
}

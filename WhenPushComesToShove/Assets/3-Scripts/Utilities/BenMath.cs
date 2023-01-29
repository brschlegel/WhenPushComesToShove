using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BenMath 
{
    //https://math.stackexchange.com/questions/227481/x-points-around-a-circle
    /// <summary>
    /// Returns equidistant points around a circle, starting at angle 0
    /// </summary>
    /// <param name="center">Center of circle</param>
    /// <param name="radius">Radius of circle</param>
    /// <param name="n">Number of points</param>
    /// <param name="angleOffset">Angle offset of points, RADIANS</param>
    /// <returns></returns>
    public static List<Vector2> GetEquidistantPoints(Vector2 center, float radius, int n,float angleOffset = 0)
    {
        List<Vector2> list = new List<Vector2>();
        for(int i = 0; i < n; i++)
        {
            Vector2 offset = new Vector2(radius * Mathf.Cos(((i * 2* Mathf.PI) / n) + angleOffset), radius * Mathf.Sin(((i * 2 * Mathf.PI)/n)) + angleOffset);
            list.Add(center + offset);
        }

        return list;
    }

    public static Vector2 Midpoint(Vector2 a, Vector2 b)
    {
        return ((b - a) / 2) + a;
    }
}

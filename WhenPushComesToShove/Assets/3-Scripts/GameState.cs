using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState 
{
    static public List<Transform> players = new List<Transform>();
    public static LevelType currentRoomType;
    public static bool damageEnabled = true;
    public static int[] playerScores = new int[4];

    //Global Modifiers
    public static float dragModifier = 1;
    //Player modifiers
    public static float playerSpeedModifier = 1;
    public static float playerAccelModifier = 1;
    public static float playerChargeModifier = 1;

    static public Transform GetNearestPlayer(Transform transform, bool onlyAlive = true)
    {
        float dist = float.PositiveInfinity;
        Transform closest = null;
        foreach (Transform t in GameState.players)
        {
            //Run only if player is alive
            if (!t.GetComponentInChildren<Health>().dead || !onlyAlive)
            {
                float temp = Vector2.Distance(transform.position, t.position);
                if (temp < dist)
                {
                    dist = temp;
                    closest = t;
                }
            }
        }
        return closest;
    }
}

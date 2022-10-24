using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState 
{
   static public List<Transform> players = new List<Transform>();
    public static LevelType currentRoomType;

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

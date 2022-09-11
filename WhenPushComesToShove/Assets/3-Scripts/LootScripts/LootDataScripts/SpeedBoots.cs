using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoots : LootData
{
    /// <summary>
    /// Doubles the players current move speed
    /// </summary>
    public override void Action()
    {
        if (playerRef != null)
        {
            playerRef.GetComponentInChildren<PlayerMovementScript>().moveSpeed *= 2;
        }
    }
}

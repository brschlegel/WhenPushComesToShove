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
        if (player != null)
        {
            player.GetComponentInChildren<PlayerMovementScript>().moveSpeed *= 2;
        }
    }
}

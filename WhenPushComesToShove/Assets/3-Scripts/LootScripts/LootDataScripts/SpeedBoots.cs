using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoots : LootData
{
    [SerializeField] private float percentSpeedIncrease = .5f;

    /// <summary>
    /// Doubles the players current move speed
    /// </summary>
    public override void Action()
    {
        if (playerRef != null)
        {
            PlayerMovementScript move = playerRef.GetComponentInChildren<PlayerMovementScript>();
            move.moveSpeed *= (1 + percentSpeedIncrease);

            if (move.moveSpeed > move.maxMoveSpeed)
            {
                move.moveSpeed = move.maxMoveSpeed;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnduranceBuff : LootData
{
    [SerializeField] private float percentMassIncrease = .2f;
    [SerializeField] public float maxMass = 100;

    /// <summary>
    /// Increases the players mass
    /// </summary>
    public override void Action()
    {
        if (playerRef != null)
        {
            KnockbackReciever reciever = playerRef.GetComponentInChildren<KnockbackReciever>();
            playerRef.GetComponent<Rigidbody2D>().mass *= Mathf.Clamp(1 + percentMassIncrease, 0, maxMass);
        
        }
    }
}

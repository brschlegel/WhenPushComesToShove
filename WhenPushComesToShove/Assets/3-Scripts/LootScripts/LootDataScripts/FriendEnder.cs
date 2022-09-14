using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendEnder : LootData
{
    
    public override void Action()
    {
        playerRef.GetComponentInChildren<DamageHitHandler>().tagsToIgnore.Remove("Shove");
    }
}

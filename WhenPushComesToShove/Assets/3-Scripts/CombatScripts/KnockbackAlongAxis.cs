using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackAlongAxis : KnockbackData
{
    [SerializeField]
    private Axis axis;
    public override Vector2 GetDirection(HitEvent e)
    {
        if(axis == Axis.XAxis)
        {
            return e.hitbox.transform.right;
        }
        else
        {
            return e.hitbox.transform.up;
        }
    }
}

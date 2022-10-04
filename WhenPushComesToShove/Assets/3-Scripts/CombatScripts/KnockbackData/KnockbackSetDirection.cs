using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackSetDirection : KnockbackData
{
    [HideInInspector] public Vector2 direction;
    public override Vector2 GetDirection(HitEvent e)
    {
        return direction.normalized;
    }
}

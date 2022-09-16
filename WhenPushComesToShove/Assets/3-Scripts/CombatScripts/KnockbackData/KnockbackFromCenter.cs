using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackFromCenter : KnockbackData
{
    public override Vector2 GetDirection(HitEvent e)
    {
        return (e.hurtbox.transform.position - e.hitbox.transform.position).normalized;
    }
}

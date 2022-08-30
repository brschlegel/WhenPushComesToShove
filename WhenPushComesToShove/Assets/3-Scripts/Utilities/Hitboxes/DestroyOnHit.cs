using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : HitHandler
{
    public override void RecieveHit(HitEvent e)
    {
        Destroy(gameObject);
    }
}

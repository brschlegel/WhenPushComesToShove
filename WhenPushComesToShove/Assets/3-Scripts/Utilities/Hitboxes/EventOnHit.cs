using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnHit : HitHandler
{

    public UnityEvent onHit;

    public override void ReceiveHit(HitEvent e )
    {
        onHit?.Invoke();
    }

}

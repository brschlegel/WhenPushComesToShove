using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void OnHit(HitEvent e);
public class EventOnHit : HitHandler
{

   
    public event OnHit onHit;

    public override void ReceiveHit(HitEvent e )
    {
        onHit?.Invoke(e);
    }

}

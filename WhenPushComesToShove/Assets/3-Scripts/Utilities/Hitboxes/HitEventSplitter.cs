using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Copies and splits hitevent amoung its outputs, allowing each hit handler to be small and focused
public class HitEventSplitter : HitHandler
{
   public List<HitHandler> outputs;

   public override void ReceiveHit(HitEvent e)
   {
        foreach(HitHandler handler in outputs)
        {
            handler.ProcessHit(e);
        }
   }
}

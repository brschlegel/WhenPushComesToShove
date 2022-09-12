using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

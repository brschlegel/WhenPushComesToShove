using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIdle : State
{
   public void Update()
   {
        if(GameState.players.Count >0)
        {
            InvokeOnStateExit(true);
            this.enabled = false;
        }
   }
}

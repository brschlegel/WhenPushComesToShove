using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbleOnHit : HitHandler
{
    public override void ReceiveHit(HitEvent e)
    {
        //Rather than getting messy with tags just going to ensure that only rumbles when both players are either dead or not
        if (e.hurtbox.owner.TryGetComponent<PlayerInputHandler>(out PlayerInputHandler handler))
        {
            bool player1Death = e.hitbox.owner.GetComponentInChildren<PlayerInputHandler>().playerConfig.IsDead;
            bool player2Death = handler.playerConfig.IsDead;

            if (player1Death == player2Death)
            {
                ControllerRumble rumble = e.hitbox.owner.GetComponentInChildren<ControllerRumble>();
                rumble.RumbleConstant(.1f, .3f, .1f);
            }
        }
        else
        {
            ControllerRumble rumble = e.hitbox.owner.GetComponentInChildren<ControllerRumble>();
            rumble.RumbleConstant(.1f, .3f, .1f);
        }





    }
}

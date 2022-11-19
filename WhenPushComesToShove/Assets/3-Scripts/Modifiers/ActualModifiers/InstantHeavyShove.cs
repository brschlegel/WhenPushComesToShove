using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantHeavyShove : BaseModifier
{
    public override void Init()
    {
        GameState.playerChargeModifier = 10.0f;
    }

    public override void CleanUp()
    {
        GameState.playerChargeModifier = 1;
    }
}

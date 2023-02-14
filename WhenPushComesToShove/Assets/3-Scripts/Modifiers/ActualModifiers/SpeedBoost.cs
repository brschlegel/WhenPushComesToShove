using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : BaseModifier
{
    [SerializeField]
    private float boostSpeedAmount;
    [SerializeField]
    private float boostAccelAmount;

    public override void Init()
    {
        GameState.playerSpeedModifier = boostSpeedAmount;
        GameState.playerAccelModifier = boostAccelAmount;
    }

    public override void CleanUp()
    {
        GameState.playerSpeedModifier = 1;
        GameState.playerAccelModifier = 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyFloors : BaseModifier
{

    [SerializeField]
    private float dragModifier;
    //Yeah, this is a little strange, but I don't want to conflict with the speed boost modifier
    [Tooltip("Multiplies the modifier by this value")]
    [SerializeField]
    private float playerAccelModifierMult;
    public override void Init()
    {
        GameState.dragModifier = dragModifier;
        GameState.playerAccelModifier *= playerAccelModifierMult;

        //Make changes immediate
        foreach(Transform t in GameState.players)
        {
            t.GetComponentInChildren<ProjectileMode>().enabled = true;
        }


    }

    public override void CleanUp()
    {
        GameState.dragModifier = 1;
        GameState.playerAccelModifier /= playerAccelModifierMult;

        foreach(Transform t in GameState.players)
        {
            t.GetComponentInChildren<ProjectileMode>().enabled = true;
        }
    }
}

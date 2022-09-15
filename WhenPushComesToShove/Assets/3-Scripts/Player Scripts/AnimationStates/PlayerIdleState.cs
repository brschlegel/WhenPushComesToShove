using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State
{
    void OnEnable()
    {
        anim.Play("Base Layer.AN_Player_Idle");
    }
}

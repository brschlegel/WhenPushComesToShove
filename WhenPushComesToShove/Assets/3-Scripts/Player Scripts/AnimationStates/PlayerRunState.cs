using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : State
{
    void OnEnable()
    {
        anim.Play("Base Layer.AN_Player_Run");
    }
}

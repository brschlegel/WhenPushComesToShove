using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AegisIdle : State
{
    private void OnEnable()
    {
        anim.Play("Base Layer.Enemy Idle");
    }

    // Update is called once per frame
    void Update()
    {
        if(GameState.players.Count > 0)
        {
            InvokeOnStateExit(true);
            enabled = false;
        }
    }
}

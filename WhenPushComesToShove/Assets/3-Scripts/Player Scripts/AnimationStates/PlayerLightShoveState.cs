using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightShoveState : State
{
    private void OnEnable()
    {
        anim.Play("Base Layer.AN_Player_LightShove");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyShoveState : State
{
    private void OnEnable()
    {
        //Play animation here 
        anim.Play("Base Layer.AN_Player_HeavyShove_Release");
    }

    private void Update()
    {
       
    }

    private void OnDisable()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyShoveState : State
{
    [HideInInspector]
    public PlayerHeavyShoveScript heavyShoveScript;

    private void OnEnable()
    {
        //Play animation here 

        switch(heavyShoveScript.chargeLevel)
        {
            case 1:
                anim.Play("AN_Player_HeavyShove_Light_Release");
            break;
            case 2: 
                anim.Play("AN_Player_HeavyShove_Medium_Release");
            break;
            case 3:
            anim.Play("AN_Player_HeavyShove_Heavy_Release");
            break;
            default:
            Debug.Log("Wrong thing!");
            break;
        }
    }

    private void Update()
    {
       
    }

    private void OnDisable()
    {
        
    }
}

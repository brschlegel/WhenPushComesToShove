using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Transitions out are handled by events 
public class PlayerChargeState : State
{
     // Start is called before the first frame update
    void OnEnable()
    {
        anim.Play("Base Layer.AN_Player_HeavyShove_BuildUp");
    }
}

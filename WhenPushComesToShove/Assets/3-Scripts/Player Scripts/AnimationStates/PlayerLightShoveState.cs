using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightShoveState : State
{
    [HideInInspector]
    public ParticleSystem shoveExclamation;
    private void OnEnable()
    {
        anim.Play("Base Layer.AN_Player_LightShove");
        //shoveExclamation.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

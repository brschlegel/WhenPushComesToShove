using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : State
{
    [HideInInspector]
    public VelocitySetter vs;

    private void OnEnable()
    {
        //Play animation here 
    }

    private void Update()
    {
        if(vs.QuerySource("playerDash", out Vector2 dash))
        {
            if(dash.magnitude <= .01f)
            {
                this.enabled = false;
                InvokeOnStateExit(true);
            }
        }
    }

    private void OnDisable()
    {
        
    }
}

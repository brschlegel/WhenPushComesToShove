using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllGasNoBrakes : EventModifier
{
    Dictionary<PlayerMovementScript, Vector2> inputs;
    public override void Init()
    {
        key = "MoveInputRequested";
        inputs = new Dictionary<PlayerMovementScript, Vector2>();
        base.Init();
    }

    protected override void OnEvent(MessageArgs args)
    {
        PlayerMovementScript movement = (PlayerMovementScript)args.objectArg;
        Vector2 providedInput = (Vector2)args.vectorArg;
        Vector2 oldInput = Vector2.zero;

        //If we have a recorded input, store that
        if(inputs.ContainsKey(movement))
        {
            oldInput = inputs[movement];
        }
        //If we don't record this input
        else
        {
            inputs.Add(movement, providedInput); 
        }

        //If we have an input, max it out
        if(providedInput.magnitude > 0)
        {
            inputs[movement] = providedInput.normalized;
        }
        //If we dont have an input, use the old maxed input
        else
        {
            inputs[movement] = oldInput;
        }

        movement.SetMoveInputVector(inputs[movement]);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

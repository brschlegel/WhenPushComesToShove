using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class Hitstun : MonoBehaviour
{
    
    private Rigidbody2D rb;

    [SerializeField]
    private VelocitySetter vs;
    private PlayerInputHandler inputHandler;
    [SerializeField]
    private float hitstunThreshold;

    private void Start()
    {
        rb = vs.GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }
    private void FixedUpdate()
    {
        float movementMagnitude = vs.sources["playerMovement"].magnitude;
       
        if(rb.velocity.magnitude - movementMagnitude >= hitstunThreshold)
        {
            inputHandler.ForceLockMovement();
        }
        else
        {
            inputHandler.ForceUnlockMovement();
        }
    }
}

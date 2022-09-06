using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class EnemyHitstun : MonoBehaviour
{  
    private Rigidbody2D rb;

    [SerializeField]
    private VelocitySetter vs;
    private MovementController movement;
    [SerializeField]
    private float hitstunThreshold;

    private void Start()
    {
        rb = vs.GetComponent<Rigidbody2D>();
        movement = GetComponent<MovementController>();
    }
    private void FixedUpdate()
    {
        if (vs.QuerySource("Move", out Vector2 vel))
        {
            float movementMagnitude = vel.magnitude;

            if (rb.velocity.magnitude - movementMagnitude >= hitstunThreshold)
            {
                movement.LockMovement();
            }
            else
            {
                movement.UnlockMovement();
            }
        }
    }
}

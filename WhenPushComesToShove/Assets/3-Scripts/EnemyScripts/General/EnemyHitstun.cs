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
    public bool inHitstun;

    private void Start()
    {
        rb = vs.GetComponent<Rigidbody2D>();
        movement = GetComponent<MovementController>();
    }
    private void Update()
    {
        if (vs.QuerySource("Move", out Vector2 vel))
        {
            float movementMagnitude = vel.magnitude;

            if (rb.velocity.magnitude - movementMagnitude >= hitstunThreshold)
            {
                if(!inHitstun)
                    movement.LockMovement();
                inHitstun = true;
            }
            else
            {
                if(inHitstun)
                    movement.UnlockMovement();
                inHitstun = false;
            }
        }
    }
}

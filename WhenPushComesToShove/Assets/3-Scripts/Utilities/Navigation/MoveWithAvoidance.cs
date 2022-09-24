using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freya;

public class MoveWithAvoidance : Move
{


    [Header("Enemy")]
    public Vector2 target;
    public bool movementLocked;

    [Header("Avoidance")]
    public float maxLookAhead;
    public int numSeeRays;
    public float raySeperation;
    public float collisionCorrectionScalar;

    private Vector2 collisionCorrection;

    public Vector3 GetMovementDirection(Vector2 target)
    {
        Vector2 currentVelocity = pMode.Velocity;
        Vector2 movement = collisionCorrection;
        Vector2 currentPos = transform.position;
        Vector2 toTarget = (target - currentPos).normalized;

        RaycastHit2D toTargetHit = Physics2D.Raycast(transform.position, toTarget, maxLookAhead, 1 << LayerMask.NameToLayer("Obstacle"));
        //If path to the target is open, take it
        if(toTargetHit.collider == null)
        {
            Debug.DrawLine(transform.position, transform.position + (Vector3)toTarget.normalized * maxLookAhead, Color.blue);
            return (collisionCorrection + toTarget).normalized;
        }

        for(int i = 0; i < numSeeRays; i++)
        {
            float sign = (i % 2 == 0) ? -1 : 1;
            float delta = sign * ((i+1) / 2) * raySeperation;
            //If our velocity is zero, then use to target to try and move from
            Vector2 baseVector = pMode.Velocity.sqrMagnitude >= .01f ? pMode.Velocity : toTarget;
            Vector2 v  = MathfsExtensions.Rotate(baseVector, Freya.MathfsExtensions.DegToRad(delta));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, v.normalized, maxLookAhead, 1 << LayerMask.NameToLayer("Obstacle"));
            Color c = Color.green;
            if(hit.collider != null)
            {
                c = Color.red;
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position +  (Vector3)v.normalized * maxLookAhead,c );
                return (collisionCorrection + v).normalized;
            }
         

        }
        return new Vector2(-toTarget.y, toTarget.x).normalized;
    }

    public void FixedUpdate()
    {
        if (!movementLocked)
        {
            //Keep movement locked if in projectile mode
            if(pMode.enabled)
            {
                movementLocked = true;
                return;
            }
            Vector2 v = GetMovementDirection(target);
            pMode.AddForce(GetForce(v));
        }
       
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        collisionCorrection = collision.contacts[0].normal * collisionCorrectionScalar;
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        collisionCorrection = Vector2.zero;
    }
}

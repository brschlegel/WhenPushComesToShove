using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the movement input action

public class PlayerMovementScript : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public AnimationCurve accelerationFactorFromDot;
    public float maxAccelerationForce;
    public AnimationCurve maxAccelerationForceFactorFromDot;

    //Velocity that input wants to achieve
    private Vector2 goalVel;

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 moveInputVector = Vector2.zero;
    private Vector2 aimInputVector = Vector2.zero;


    [HideInInspector] public ProjectileMode pMode;

    [HideInInspector] public Transform player;

    private void Awake()
    {
        player = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //https://www.youtube.com/watch?v=qdskE8PJy6Q&ab_channel=ToyfulGames
    private void FixedUpdate()
    {
        Vector2 unitMove = moveInputVector.normalized;

 
        //When turning 180 degrees, it takes twice as long to slow down to zero, and then speed back up in the other direction
        // when compared to running from a standstill
        //The animation curve scales the acceleration from 1-2 based on the dot product (how far away the current velocity is from the desired velocity)
        float velDot = Vector2.Dot(unitMove, goalVel.normalized);
        float accel = acceleration * accelerationFactorFromDot.Evaluate(velDot);
        //If moving but not aiming, default aim to move direction
        if (aimInputVector == Vector2.zero && unitMove != Vector2.zero)
        {
            player.right = unitMove.normalized;
        }
      
        //When we input a vector, that is our target velocity
        Vector2 desiredVel = unitMove * maxSpeed;

        //Move goal velocity towards our desired velocity by amount allowed by acceleration
        goalVel = Vector2.MoveTowards(goalVel, desiredVel, accel * Time.fixedDeltaTime);
        
        //Find out what acceleration is neccessary to reach goal velocity 
        Vector2 needAccel = (goalVel - pMode.Velocity) / Time.fixedDeltaTime;
        
        //Same deal with the animation curve as above
        needAccel = Vector2.ClampMagnitude(needAccel, maxAccelerationForce * maxAccelerationForceFactorFromDot.Evaluate(velDot));
        //Apply the force
        pMode.AddForce(needAccel * pMode.Mass);

    }

    /// <summary>
    /// Basic function to apply movement based on input
    /// </summary>
    public void Move()
    {
        moveDirection = new Vector3(moveInputVector.x, moveInputVector.y, 0);

       

        //If moving but not aiming, default aim to move direction
        if (aimInputVector == Vector2.zero && moveDirection != Vector3.zero)
        {
            player.right = moveDirection.normalized;
        }
    }

    /// <summary>
    /// Function for the PlayerInputHandler to pass in the move direction
    /// </summary>
    /// <param name="direction"></param>
    public void SetMoveInputVector(Vector2 direction)
    {
        moveInputVector = direction;
    }

    /// <summary>
    /// Function for the PlayerInputHandler to pass in the aim direction
    /// </summary>
    /// <param name="direction"></param>
    public void SetAimInputVector(Vector2 direction)
    {
        //If not aiming, set vector to zero
        if (direction == Vector2.zero)
        {
            aimInputVector = Vector2.zero;
            return;
        }

        aimInputVector = direction;

        //Rotate player to that direction
        player.right = aimInputVector.normalized;
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }

    public Vector2 GetAimDirection()
    {
        return aimInputVector;
    }

    public bool IsMoving
    {
        get{return moveInputVector.magnitude > 0;}
    }
}

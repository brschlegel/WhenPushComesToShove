using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Move : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed;
    public float acceleration;
    public AnimationCurve accelerationFactorFromDot;
    public float maxAccelerationForce;
    public AnimationCurve maxAccelerationForceFactorFromDot;
    [SerializeField]
    [HideInInspector] public ProjectileMode pMode;
   
    //Velocity that input wants to achieve
    protected Vector2 goalVel;

    private void Start()
    {
        
    }

    public virtual void Init()
    {
       
    }

    public Vector2 GetForce(Vector2 unitMove)
    {
        //When turning 180 degrees, it takes twice as long to slow down to zero, and then speed back up in the other direction
        // when compared to running from a standstill
        //The animation curve scales the acceleration from 1-2 based on the dot product (how far away the current velocity is from the desired velocity)
        float velDot = Vector2.Dot(unitMove, goalVel.normalized);
        float accel = acceleration * accelerationFactorFromDot.Evaluate(velDot);
        
        //When we input a vector, that is our target velocity
        Vector2 desiredVel = unitMove * maxSpeed;

        //Move goal velocity towards our desired velocity by amount allowed by acceleration
        goalVel = Vector2.MoveTowards(goalVel, desiredVel, accel * Time.fixedDeltaTime);
        
        //Find out what acceleration is neccessary to reach goal velocity 
        Vector2 needAccel = (goalVel - pMode.Velocity) / Time.fixedDeltaTime;
        
        //Same deal with the animation curve as above
        needAccel = Vector2.ClampMagnitude(needAccel, maxAccelerationForce * maxAccelerationForceFactorFromDot.Evaluate(velDot));
        return needAccel * pMode.Mass;
    }

    


  
   

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the movement input action

public class PlayerMovementScript : Move
{


    private Vector3 moveDirection = Vector3.zero;
    private Vector2 moveInputVector = Vector2.zero;
    private Vector2 aimInputVector = Vector2.zero;


    

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
      
        //If moving but not aiming, default aim to move direction
        if (aimInputVector == Vector2.zero && unitMove != Vector2.zero)
        {
            player.right = unitMove.normalized;
        }
      
        //Apply the force
        pMode.AddForce(GetForce(unitMove));

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

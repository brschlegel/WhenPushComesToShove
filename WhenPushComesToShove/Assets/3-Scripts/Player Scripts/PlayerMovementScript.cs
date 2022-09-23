using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the movement input action
public class PlayerMovementScript : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float maxMoveSpeed = 20f;

    private float originalSpeed;

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 moveInputVector = Vector2.zero;
    private Vector2 aimInputVector = Vector2.zero;

    [HideInInspector] public VelocitySetter vs;

    [HideInInspector] public Transform player;

    private void Awake()
    {
        player = transform.parent;
        originalSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /// <summary>
    /// Basic function to apply movement based on input
    /// </summary>
    public void Move()
    {
        moveDirection = new Vector3(moveInputVector.x, moveInputVector.y, 0);

        if (vs != null)
        {
            vs.AddSource("playerMovement", moveDirection, moveSpeed);
        }

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

    //Helper Functions for Modifying Move Speed
    public void ChangeMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void ResetMoveSpeed()
    {
        moveSpeed = originalSpeed;
    }

    public IEnumerator ChangeMoveSpeedForTime(float newSpeed, float time)
    {
        moveSpeed = newSpeed;

        yield return new WaitForSeconds(time);

        moveSpeed = originalSpeed;
    }
}

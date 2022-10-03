using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the movement input action

public class PlayerMovementScript : Move
{
    public float slowAmount = 1f;

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 moveInputVector = Vector2.zero;
    private Vector2 aimInputVector = Vector2.zero;
    [HideInInspector] public Transform player;

    private bool lockMovement = false;
    private Coroutine movementUnlockRoutine;

    #region Properties
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
        get { return moveInputVector.magnitude > 0 && !lockMovement; }
    }
    #endregion

    private void Awake()
    {
        player = transform.parent;
    }

    //https://www.youtube.com/watch?v=qdskE8PJy6Q&ab_channel=ToyfulGames
    private void FixedUpdate()
    {
        if (lockMovement)
        {
            SetMoveInputVector(Vector2.zero);
        }

        Vector2 unitMove = moveInputVector;
      
        //If moving but not aiming, default aim to move direction
        if (aimInputVector == Vector2.zero && unitMove != Vector2.zero)
        {
            player.right = unitMove.normalized;
        }
      
        //Apply the force
        pMode.AddForce(GetForce(unitMove * slowAmount));

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

    #region ChangingMoveSpeed

    //Helper Functions for Modifying Move Speed
    public void ChangeMoveSpeed(float newSpeed)
    {
        slowAmount = newSpeed;
    }

    public void ResetMoveSpeed()
    {
        slowAmount = 1f;
    }

    public IEnumerator ChangeMoveSpeedForTime(float newSpeed, float time)
    {
        slowAmount = newSpeed;

        yield return new WaitForSeconds(time);

        slowAmount = 1f;
    }
    #endregion
    #region LockingMovement
    /// <summary>
    /// Locks the player's movement for a period of time
    /// </summary>
    /// <param name="cooldown"></param>
    public void LockMovementForTime(float cooldown)
    {
        lockMovement = true;
        movementUnlockRoutine = StartCoroutine(MovementLockCooldown(cooldown));
    }

    /// <summary>
    /// A function to unlock movement after a period of time
    /// </summary>
    /// <param name="cooldown"></param>
    /// <returns></returns>
    public IEnumerator MovementLockCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        lockMovement = false;
    }

    /// <summary>
    /// Used to externally force the player's movement to lock
    /// </summary>
    public void ForceLockMovement()
    {
        if (movementUnlockRoutine != null)
        {
            StopCoroutine(movementUnlockRoutine);
        }

        lockMovement = true;
    }

    /// <summary>
    /// Used to externally force the player's movement to unlock
    /// </summary>
    public void ForceUnlockMovement()
    {
        lockMovement = false;
    }
    #endregion
}

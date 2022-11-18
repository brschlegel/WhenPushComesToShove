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

    [SerializeField] private Transform aimTriangle;

    private float fixedX;
    private float fixedY;

    private bool lockMovement = false;
    private Coroutine movementUnlockRoutine;
    private int forceMovementLocks = 0;
    private int forceAimLocks = 0;
    private Coroutine aimUnlockRoutine;

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
        fixedX = aimTriangle.eulerAngles.x;
        fixedY = aimTriangle.eulerAngles.y;
    }

    //https://www.youtube.com/watch?v=qdskE8PJy6Q&ab_channel=ToyfulGames
    private void FixedUpdate()
    {
        if (forceMovementLocks > 0)
        {
            SetMoveInputVector(Vector2.zero);
        }

        Vector2 unitMove = moveInputVector;
      
        //If moving but not aiming, default aim to move direction
        if (aimInputVector == Vector2.zero && unitMove != Vector2.zero && forceAimLocks <=  0)
        {
            player.right = unitMove.normalized;
            aimTriangle.eulerAngles = new Vector3(fixedX, fixedY, player.transform.eulerAngles.z);
        }
      
        //Apply the force
        pMode.AddForce(GetForce(unitMove * slowAmount * GameState.playerSpeedModifier, GameState.playerAccelModifier));

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
        if (forceAimLocks <= 0)
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
            aimTriangle.eulerAngles = new Vector3(fixedX, fixedY, player.transform.eulerAngles.z);
        }
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
        //lockMovement = true;
        forceMovementLocks++;
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
        //lockMovement = false;
        forceMovementLocks--;
    }

    /// <summary>
    /// Used to externally force the player's movement to lock
    /// </summary>
    public void ForceLockMovement()
    {
        if (movementUnlockRoutine != null)
        {
            forceMovementLocks--;
            StopCoroutine(movementUnlockRoutine);
        }

        //lockMovement = true;
        forceMovementLocks++;
    }

    /// <summary>
    /// Used to externally force the player's movement to unlock
    /// </summary>
    public void ForceUnlockMovement()
    {
        //lockMovement = false;
        forceMovementLocks--;
    }
    #endregion
    #region LockingAim
    /// <summary>
    /// Locks the player's movement for a period of time
    /// </summary>
    /// <param name="cooldown"></param>
    public void LockAimForTime(float cooldown)
    {
        //lockMovement = true;
        forceAimLocks++;
        aimUnlockRoutine = StartCoroutine(MovementLockCooldown(cooldown));
    }

    /// <summary>
    /// A function to unlock movement after a period of time
    /// </summary>
    /// <param name="cooldown"></param>
    /// <returns></returns>
    public IEnumerator AimLockCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        //lockMovement = false;
        forceAimLocks--;
    }

    /// <summary>
    /// Used to externally force the player's movement to lock
    /// </summary>
    public void ForceLockAim()
    {
        if (aimUnlockRoutine != null)
        {
            StopCoroutine(aimUnlockRoutine);
        }

        //lockMovement = true;
        forceAimLocks++;
    }

    /// <summary>
    /// Used to externally force the player's movement to unlock
    /// </summary>
    public void ForceUnlockAim()
    {
        //lockMovement = false;
        forceAimLocks--;
    }
    #endregion 
}

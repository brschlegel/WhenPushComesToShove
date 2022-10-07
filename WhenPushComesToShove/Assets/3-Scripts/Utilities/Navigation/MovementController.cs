using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MoveWithAvoidance))]
public class MovementController : MonoBehaviour
{
    public Vector2 Destination {get; set;}
    [SerializeField]
    [Tooltip("How far from the destination before stopping")]
    protected float destinationTolerance;
    [SerializeField]
    protected ProjectileMode pMode;

    [Header("Events")]
    [SerializeField]
    protected UnityEvent startMovingEvent;
    [SerializeField]
    protected UnityEvent endMovingEvent;

    /// <summary>
    /// Purely convience, reads from and sets the move component's speed
    /// </summary>
    /// <value></value>
    public float Speed
    {
        get
        {
            return move.maxSpeed;
        }
        set
        {
            move.maxSpeed = value;
        }
    }
    
    protected MoveWithAvoidance move;

    // Start is called before the first frame update
    void Start()
    {
        if(move == null)
        {
            Init();
        }
    }

    public void Init()
    {
        move = GetComponent<MoveWithAvoidance>();
        move.pMode = pMode;
    }

    public void LockMovement()
    {
        if(move == null)
        {
            Init();
        }
        move.movementLocked = true;
    }

    public void UnlockMovement()
    {
        if(move == null)
        {
            Init();
        }
        move.movementLocked = false;
    }

    public bool IsMoveEnabled()
    {
        return move.enabled;
    }


}

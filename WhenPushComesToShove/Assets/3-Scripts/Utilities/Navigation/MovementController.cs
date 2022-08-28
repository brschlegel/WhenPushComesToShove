using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Move))]
public class MovementController : MonoBehaviour
{
    public Vector2 Destination {get; set;}
    [SerializeField]
    [Tooltip("How far from the destination before stopping")]
    protected float destinationTolerance;

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
            return move.speed;
        }
        set
        {
            move.speed = value;
        }
    }
    [SerializeField]
    protected Move move;

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
        move = GetComponent<Move>();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the movement input action
public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 inputVector = Vector2.zero;

    [HideInInspector] public VelocitySetter vs;

    public void Awake()
    {
        vs = GetComponent<VelocitySetter>();
        vs.Init();
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
        moveDirection = new Vector3(inputVector.x, inputVector.y, 0);

        if (vs != null)
        {
            vs.AddSource("playerMovement", moveDirection, moveSpeed);
        }
    }

    /// <summary>
    /// Function for the PlayerInputHandler to pass in the move direction
    /// </summary>
    /// <param name="direction"></param>
    public void SetInputVector(Vector2 direction)
    {
        inputVector = direction;
    }
}

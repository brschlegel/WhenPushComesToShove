using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

//Script to take in player input and trigger the necessary actions
public class PlayerInputHandler : MonoBehaviour
{
    [HideInInspector] public PlayerConfiguration playerConfig;
    private PlayerMovementScript mover;

    private SpriteRenderer sr;

    private PlayerControls controls;

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    public void Init()
    {
        sr = GetComponent<SpriteRenderer>();
        mover = GetComponent<PlayerMovementScript>();
        controls = new PlayerControls();
    }

    /// <summary>
    /// Function called in InitLevel to set up the player prefab and input for the level
    /// </summary>
    /// <param name="config">The player configuration to initialize</param>
    public void InitializePlayer(PlayerConfiguration config)
    {
        playerConfig = config;
        sr.material = config.PlayerMaterial;
        playerConfig.Input.onActionTriggered += Input_onActionTriggered;

        //Assign player index to mover
        mover.playerIndex = playerConfig.PlayerIndex;
    }

    /// <summary>
    /// Checks the input used and calls the nessecary functions
    /// </summary>
    /// <param name="obj"></param>
    private void Input_onActionTriggered(CallbackContext obj)
    {
        if (obj.action.name == controls.PlayerMovement.Movement.name)
        {
            OnMove(obj);
        }
    }

    /// <summary>
    /// Sets the input vector for the movement input
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(CallbackContext context)
    {
        if (mover != null)
        {
            mover.SetInputVector(context.ReadValue<Vector2>());
        }
    }
}

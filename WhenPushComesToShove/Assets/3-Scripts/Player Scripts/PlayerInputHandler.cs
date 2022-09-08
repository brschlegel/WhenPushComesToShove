using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using System;

//Script to take in player input and trigger the necessary actions
public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private float shoveActionCooldown = 1;
    [SerializeField] private float dashActionCooldown = 1;
    [SerializeField] private float movementLockCooldown = .6f;

    [HideInInspector] public PlayerConfiguration playerConfig;

    private VelocitySetter vs;

    private PlayerMovementScript mover;
    private PlayerLightShoveScript lightShoveScript;
    private PlayerHeavyShoveScript heavyShoveScript;
    private PlayerDashScript dashScript;

    [HideInInspector] public bool performingAction = false;
    private bool lockMovement = false;

    private SpriteRenderer sr;

    private PlayerControls controls;

    public Action onSelect;

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    public void Init()
    {
        sr = GetComponentInParent<SpriteRenderer>();
        controls = new PlayerControls();

        vs = GetComponentInParent<VelocitySetter>();
        vs.Init();

        mover = GetComponent<PlayerMovementScript>();
        lightShoveScript = GetComponent<PlayerLightShoveScript>();
        heavyShoveScript = GetComponent<PlayerHeavyShoveScript>();
        dashScript = GetComponent<PlayerDashScript>();

        //Assign Velocity Setter to Necessary Input Scripts
        mover.vs = vs;
        dashScript.vs = vs;
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
    }

    /// <summary>
    /// Checks the input used and calls the nessecary functions
    /// </summary>
    /// <param name="obj"></param>
    private void Input_onActionTriggered(CallbackContext obj)
    {
        //Movement
        if (obj.action.name == controls.PlayerMovement.Movement.name)
        {
            OnMove(obj);
        }
        //Select
        else if (obj.action.name == controls.PlayerMovement.Select.name)
        {
            if (onSelect != null)
            {
                onSelect();
            }
        }
        //Light Shove
        else if (obj.action.name == controls.PlayerMovement.LightShove.name)
        {
            if (!performingAction)
            {
                LockAction(shoveActionCooldown);
                lightShoveScript.onLightShove();
            }
        }
        //Heavy Shove
        else if (obj.action.name == controls.PlayerMovement.HeavyShove.name)
        {
            if (!performingAction)
            {
                LockAction(shoveActionCooldown);
                heavyShoveScript.onHeavyShove();
            }
        }
        //Dash
        else if (obj.action.name == controls.PlayerMovement.Dash.name)
        {
            if (!performingAction)
            {
                LockAction(dashActionCooldown);
                LockMovement(movementLockCooldown);
                dashScript.OnDash(DetermineDashDirection());
            }
        }
        //Aim
        else if (obj.action.name == controls.PlayerMovement.Aim.name)
        {
            OnAim(obj);
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
            if (lockMovement)
            {
                mover.SetMoveInputVector(Vector2.zero);
                return;
            }

            mover.SetMoveInputVector(context.ReadValue<Vector2>());
        }
    }

    /// <summary>
    /// Sets the input vector for aim input
    /// </summary>
    /// <param name="context"></param>
    public void OnAim(CallbackContext context)
    {
        if (mover != null)
        {
            mover.SetAimInputVector(context.ReadValue<Vector2>());
        }
    }

    /// <summary>
    /// Helper function to determine if the direction of a dash
    /// </summary>
    /// <returns></returns>
    public Vector2 DetermineDashDirection()
    {
        Vector2 direction = Vector2.zero;

        if (mover.GetMoveDirection() != Vector3.zero)
        {
            direction = mover.GetMoveDirection();
        }
        else if (mover.GetAimDirection() != Vector2.zero)
        {
            direction = mover.GetAimDirection();
        }
        else
        {
            direction = mover.player.right;
        }

        return direction;
    }

    /// <summary>
    /// Locks the player from performing an action for a period of time
    /// </summary>
    /// <param name="cooldown"></param>
    public void LockAction(float cooldown)
    {
        performingAction = true;
        StartCoroutine(ActionCooldown(cooldown));
    }

    /// <summary>
    /// Locks the player's movement for a period of time
    /// </summary>
    /// <param name="cooldown"></param>
    public void LockMovement(float cooldown)
    {
        lockMovement = true;
        StartCoroutine(MovementLockCooldown(cooldown));
    }

    /// <summary>
    /// Used to externally force the player's movement to lock
    /// </summary>
    public void ForceLockMovement()
    {
        lockMovement = true;
    }

    /// <summary>
    /// Used to externally force the player's movement to unlock
    /// </summary>
    public void ForceUnlockMovement()
    {
        lockMovement = false;
    }

    /// <summary>
    /// A fuction to prevent players from performing an action for some time
    /// </summary>
    /// <returns></returns>
    public IEnumerator ActionCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        performingAction = false;
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
    /// Helper function to clear the current action assigned to select
    /// </summary>
    public void ClearSelectAction()
    {
        onSelect = null;
    }
}

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
    private float lightShoveActionCooldown = .5f;
    private float heavyShoveActionCooldown = 1;
    [SerializeField] private float dashActionCooldown = 1;
    [SerializeField] private float movementLockCooldown = .6f;
    [SerializeField] private float selectActionCooldown = .5f;

    [HideInInspector] public PlayerConfiguration playerConfig;

    [SerializeField]
    private ProjectileMode pMode;

    private PlayerMovementScript mover;
    private PlayerLightShoveScript lightShoveScript;
    private PlayerHeavyShoveScript heavyShoveScript;
    private PlayerDashScript dashScript;

    
    [HideInInspector] public bool heavyShoveIsCharging = false;
    [HideInInspector] public float heavyShoveCharge = 0;
    [SerializeField] public float heavyShoveChargeTime = 1;

    [HideInInspector] public bool performingAction = false;
    private bool lockMovement = false;
    private Coroutine movementUnlockRoutine;
    public Action onLightShoveComplete;
    public Action onHeavyShoveComplete;
    public Action onHeavyShoveCharge;

    [HideInInspector] public bool dead = false;

    [HideInInspector] public SpriteRenderer sr;
    private Animator anim;

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
        anim = GetComponentInParent<Animator>();
        controls = new PlayerControls();

        mover = GetComponent<PlayerMovementScript>();
        lightShoveScript = GetComponent<PlayerLightShoveScript>();
        heavyShoveScript = GetComponent<PlayerHeavyShoveScript>();
        dashScript = GetComponent<PlayerDashScript>();

        //Assign Velocity Setter to Necessary Input Scripts
        mover.pMode = pMode;
        dashScript.pMode = pMode;
        lightShoveActionCooldown = lightShoveScript.cooldown;
        heavyShoveActionCooldown = heavyShoveScript.cooldown;

    }

    private void Update()
    {
        if (heavyShoveIsCharging)
        {
            heavyShoveCharge += Time.deltaTime;
        }
    }

    /// <summary>
    /// Function called in InitLevel to set up the player prefab and input for the level
    /// </summary>
    /// <param name="config">The player configuration to initialize</param>
    public void InitializePlayer(PlayerConfiguration config)
    {
        playerConfig = config;
        anim.runtimeAnimatorController = config.PlayerAnimations;
        sr.material = config.Outline;
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
        else if (obj.action.name == controls.PlayerMovement.Select.name && !playerConfig.IsDead)
        {
            if (onSelect != null && !performingAction && !heavyShoveIsCharging)
            {
                LockAction(selectActionCooldown, null);
                onSelect();
            }
        }
        //Light Shove
        else if (obj.action.name == controls.PlayerMovement.LightShove.name && !playerConfig.IsDead)
        {
            if (!performingAction && !heavyShoveIsCharging && obj.started)
            {
                LockAction(lightShoveActionCooldown, onLightShoveComplete);
                StartCoroutine(mover.ChangeMoveSpeedForTime(lightShoveScript.speedDecrease, lightShoveActionCooldown));

                lightShoveScript.onLightShove();
            }
        }
        //Heavy Shove Charge
        else if (obj.action.name == controls.PlayerMovement.HeavyShove.name && !playerConfig.IsDead)
        {
            if (!performingAction && obj.started)
            {
                //Assign Release Method
                obj.action.canceled += WaitForChargeRelease;
                onHeavyShoveCharge?.Invoke();

                heavyShoveIsCharging = true;
                heavyShoveCharge = 0;
                mover.ChangeMoveSpeed(heavyShoveScript.speedDecrease);
                //ForceLockMovement();
                Debug.Log("Charge");
            }
        }
        //Dash
        else if (obj.action.name == controls.PlayerMovement.Dash.name)
        {
            if (!performingAction && !heavyShoveIsCharging)
            {
                LockAction(dashActionCooldown, null);
                //LockMovement(movementLockCooldown);
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
    /// Interrupts charging if hit
    /// </summary>
    public void InterruptCharge()
    {
        if(heavyShoveIsCharging)
        {
            heavyShoveIsCharging = false;
            heavyShoveCharge = 0;
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
    public void LockAction(float cooldown, Action onComplete)
    {
        performingAction = true;
        StartCoroutine(ActionCooldown(cooldown, onComplete));
    }

    /// <summary>
    /// Locks the player's movement for a period of time
    /// </summary>
    /// <param name="cooldown"></param>
    public void LockMovement(float cooldown)
    {
        lockMovement = true;
        movementUnlockRoutine = StartCoroutine(MovementLockCooldown(cooldown));
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

    /// <summary>
    /// A fuction to prevent players from performing an action for some time
    /// </summary>
    /// <returns></returns>
    public IEnumerator ActionCooldown(float cooldown, Action onComplete)
    {
        yield return new WaitForSeconds(cooldown);
        performingAction = false;
        if(onComplete != null)
        {
            onComplete?.Invoke();
        }
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

    /// <summary>
    /// Function called when the player release the heavy shove button
    /// </summary>
    /// <param name="obj"></param>
    public void WaitForChargeRelease(CallbackContext obj)
    {
        //ForceUnlockMovement();
        mover.ResetMoveSpeed();
        heavyShoveIsCharging = false;

        if (heavyShoveCharge >= heavyShoveChargeTime)
        {
            Debug.Log("Heavy Shove");
            LockAction(heavyShoveActionCooldown, onHeavyShoveComplete);
            heavyShoveScript.onHeavyShove();
        }

        heavyShoveCharge = 0;
    }
}

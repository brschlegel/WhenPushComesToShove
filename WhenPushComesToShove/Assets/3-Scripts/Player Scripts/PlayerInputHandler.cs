using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using System;

public delegate void PlayerEvent(int index);


//Script to take in player input and trigger the necessary actions
public class PlayerInputHandler : MonoBehaviour
{
    public event PlayerEvent onSelect;
    public Action onLightShoveComplete;
    public Action onHeavyShoveComplete;
    public Action onHeavyShoveCharge;
    public Action onLeftEmote;
    public Action onLeftEmoteEnd;
    public bool movementPaused = false;

    [HideInInspector] public PlayerConfiguration playerConfig;
    [HideInInspector] public ControllerRumble rumble;
    [HideInInspector] public bool performingAction = false;
    [HideInInspector] public SpriteRenderer sr;

    [SerializeField] private float selectActionCooldown = .5f;
    [SerializeField] private ProjectileMode pMode;
    [SerializeField] private PlayerComponentReferences references;

    private PlayerMovementScript mover;
    private PlayerLightShoveScript lightShoveScript;
    private PlayerHeavyShoveScript heavyShoveScript;
    private PlayerDashScript dashScript;
    private Animator anim;
    private PlayerControls controls;
    private bool buttonMashing = false;
    private int buttonMashedNum = 0;
    private bool emotesRunning = false;

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

        rumble = GetComponent<ControllerRumble>();

        //Assign Velocity Setter to Necessary Input Scripts
        mover.pMode = pMode;
        dashScript.pMode = pMode;


        //On Join Controller Rumble;
        rumble.RumbleLinear(1, 0, 1, 0, .5f, false);

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
            if (mover != null)
            {
                if (movementPaused)
                {
                    mover.SetMoveInputVector(new Vector2(0,0));
                }
                else
                {
                    mover.SetMoveInputVector(obj.ReadValue<Vector2>());
                }
            }
        }
        //Select
        else if (obj.action.name == controls.PlayerMovement.Select.name && !playerConfig.IsDead)
        {
            if (onSelect != null && !performingAction && !heavyShoveScript.heavyShoveIsCharging)
            {
                if (emotesRunning)
                {
                    emotesRunning = false;
                    anim.StopPlayback();
                }

                LockAction(selectActionCooldown, null);
                onSelect.Invoke(playerConfig.PlayerIndex);
            }
        }
        //Light Shove
        else if (obj.action.name == controls.PlayerMovement.LightShove.name)
        {
            if (!performingAction && !heavyShoveScript.heavyShoveIsCharging && !movementPaused)
            {
                emotesRunning = false;
                lightShoveScript.OnLightShoveStart(obj);
            }
            
        }
        //Heavy Shove Charge
        else if (obj.action.name == controls.PlayerMovement.HeavyShove.name)
        {
            if (!performingAction && !movementPaused)
            {
                emotesRunning = false;
                heavyShoveScript.OnHeavyShoveStart(obj);
            }
        }
        //Dash
        else if (obj.action.name == controls.PlayerMovement.Dash.name && !movementPaused)
        {
            if (!performingAction)
            {
                if (emotesRunning)
                {
                    emotesRunning = false;
                    anim.StopPlayback();
                }

                if (heavyShoveScript.heavyShoveIsCharging)
                {
                    heavyShoveScript.InterruptCharge();
                }

                dashScript.OnDash(obj);
            }
        }
        //Aim
        else if (obj.action.name == controls.PlayerMovement.Aim.name)
        {
            if (mover != null)
            {
                mover.SetAimInputVector(obj.ReadValue<Vector2>());
            }
        }
        //Emotes
        else if (obj.action.name == controls.PlayerMovement.EmoteDown.name)
        {
            if (references.circleVFX != null)
            {
                references.circleVFX.gameObject.SetActive(true);
            }
        }
        else if (obj.action.name == controls.PlayerMovement.EmoteLeft.name)
        {
            if (!performingAction)
            {
                if (emotesRunning)
                {
                    if (onLeftEmoteEnd != null)
                    {
                        onLeftEmoteEnd.Invoke();
                        emotesRunning = false;
                    }

                }
                else
                {
                    if (onLeftEmote != null)
                    {
                        onLeftEmote.Invoke();
                        emotesRunning = true;
                    }
                }

                LockAction(selectActionCooldown, null);
            }
        }
    }

    /// <summary>
    /// Helper function to clear the current action assigned to select
    /// </summary>
    public void ClearSelectAction()
    {
        onSelect = null;
    }

    #region LockActions
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
    /// A fuction to prevent players from performing an action for some time
    /// </summary>
    /// <returns></returns>
    public IEnumerator ActionCooldown(float cooldown, Action onComplete)
    {
        yield return new WaitForSeconds(cooldown);
        performingAction = false;
        if (onComplete != null)
        {
            onComplete?.Invoke();
        }
    }
    #endregion
}

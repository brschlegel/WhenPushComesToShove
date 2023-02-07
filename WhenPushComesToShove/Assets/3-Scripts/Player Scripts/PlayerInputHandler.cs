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
    [SerializeField] private float selectActionCooldown = .5f;

    [HideInInspector] public PlayerConfiguration playerConfig;

    [SerializeField]
    private ProjectileMode pMode;

    public bool movementPaused = false;

    private PlayerMovementScript mover;
    private PlayerLightShoveScript lightShoveScript;
    private PlayerHeavyShoveScript heavyShoveScript;
    private PlayerDashScript dashScript;

    [HideInInspector] public ControllerRumble rumble;

    [HideInInspector] public bool performingAction = false;

    public Action onLightShoveComplete;
    public Action onHeavyShoveComplete;
    public Action onHeavyShoveCharge;


    [HideInInspector] public SpriteRenderer sr;
    private Animator anim;

    private PlayerControls controls;

    public event PlayerEvent onSelect;

    public GameObject crownBox;

    private bool buttonMashing = false;
    private int buttonMashedNum = 0;

    public ParticleSystem circleVFX;

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

    private void Update()
    {
        if (emotesRunning)
        {
            PlayChargeAnimation();
        }
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
            if (circleVFX != null)
            {
                circleVFX.gameObject.SetActive(true);
            }
        }
        else if (obj.action.name == controls.PlayerMovement.EmoteLeft.name)
        {
            if (!performingAction)
            {
                if (emotesRunning)
                {
                    anim.Play("Base Layer.AN_Player_Idle", -1);
                    emotesRunning = false;

                }
                else
                {
                    anim.Play("Base Layer.AN_Player_ChestHuff", -1);
                    emotesRunning = true;
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

    private void PlayChargeAnimation()
    {
        anim.Play("Base Layer.AN_Player_ChestHuff", -1);
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

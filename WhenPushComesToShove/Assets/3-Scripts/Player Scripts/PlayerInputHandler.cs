using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

//Script to take in player input and trigger the necessary actions
public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private int actionCooldown = 1;

    [HideInInspector] public PlayerConfiguration playerConfig;

    private VelocitySetter vs;

    private PlayerMovementScript mover;
    private PlayerLightShoveScript lightShoveScript;
    private PlayerHeavyShoveScript heavyShoveScript;
    private PlayerDashScript dashScript;

    [HideInInspector] public bool performingAction = false;

    private SpriteRenderer sr;

    private PlayerControls controls;

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
        if (obj.action.name == controls.PlayerMovement.Movement.name)
        {
            OnMove(obj);
        }
        else if (obj.action.name == controls.PlayerMovement.Select.name)
        {
            Debug.Log("Player " + playerConfig.PlayerIndex + " Performed the Select Action");
        }
        else if (obj.action.name == controls.PlayerMovement.LightShove.name)
        {
            if (!performingAction)
            {
                performingAction = true;
                lightShoveScript.OnLightShove(playerConfig.PlayerIndex);
                StartCoroutine(ActionCooldown());
            }
        }
        else if (obj.action.name == controls.PlayerMovement.HeavyShove.name)
        {
            if (!performingAction)
            {
                performingAction = true;
                heavyShoveScript.OnHeavyShove(playerConfig.PlayerIndex);
                StartCoroutine(ActionCooldown());
            }
        }
        else if (obj.action.name == controls.PlayerMovement.Dash.name)
        {
            if (!performingAction)
            {
                performingAction = true;
                dashScript.OnDash(playerConfig.PlayerIndex);
                StartCoroutine(ActionCooldown());
            }
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

    public IEnumerator ActionCooldown()
    {
        yield return new WaitForSeconds(actionCooldown);
        performingAction = false;
    }
}

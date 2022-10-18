using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerHeavyShoveScript : MonoBehaviour
{
    [HideInInspector] public bool heavyShoveIsCharging = false;
    [HideInInspector] public float heavyShoveCharge = 0;
    [SerializeField] public float lowTierChargeTime = .3f;
    [SerializeField] public float midTierChargeTime = .5f;
    [SerializeField] public float highTierChargeTime = 1;

    [HideInInspector]public int chargeLevel = 0;


    public float speedDecrease = .2f;

    public Action onHeavyShove;
    public Action onHeavyCharge;
    public Action onHeavyFail;

    public Hitbox hitbox;
    private Collider2D collider;

    public float cooldown;
    public float hitBoxCooldown;
    private SpriteRenderer sr;

    private PlayerInputHandler handler;
    private PlayerMovementScript mover;

    public void Start()
    {
        collider = hitbox.gameObject.GetComponent<Collider2D>();
        collider.enabled = false;
        sr = hitbox.gameObject.GetComponent<SpriteRenderer>();
        sr.enabled = false;

        mover = GetComponent<PlayerMovementScript>();
        handler = GetComponent<PlayerInputHandler>();

        EnableBaseHeavyShove();

        LevelManager.onNewRoom += InterruptCharge;
    }

    public void Update()
    {
        if (heavyShoveIsCharging)
        {
            heavyShoveCharge += Time.deltaTime;

            if (heavyShoveCharge >= highTierChargeTime)
            {
                chargeLevel = 3;
            }
            else if (heavyShoveCharge >= midTierChargeTime)
            {
                chargeLevel = 2;
            }
            else if(heavyShoveCharge >= lowTierChargeTime)
            {
                chargeLevel = 1;
            }
            else
            {
                chargeLevel = 0;
            }

            Debug.Log(chargeLevel);
        }
    }

    /// <summary>
    /// Triggers the Heavy Shove
    /// </summary>
    /// <param name="context"></param>
    public void OnHeavyShoveStart(CallbackContext context)
    {
        if (context.started)
        {
            //Assign Release Method
            context.action.canceled += WaitForChargeRelease;

            if (!handler.playerConfig.IsDead)
            {
                handler.rumble.RumbleLinear(0, .5f, 0, .5f, highTierChargeTime, true);
            }

            heavyShoveIsCharging = true;
            heavyShoveCharge = 0;
            chargeLevel = 0;
            mover.ChangeMoveSpeed(speedDecrease);
            onHeavyCharge?.Invoke();
        }
    }

    /// <summary>
    /// Function called when the player release the heavy shove button
    /// </summary>
    /// <param name="obj"></param>
    public void WaitForChargeRelease(CallbackContext obj)
    {
        mover.ResetMoveSpeed();
        heavyShoveIsCharging = false;

        if (heavyShoveCharge >= lowTierChargeTime)
        {
            LoggingInfo.instance.heavyShoveUses[handler.playerConfig.PlayerIndex] += 1;
            handler.LockAction(cooldown, handler.onHeavyShoveComplete);
            onHeavyShove();
        }
        else
        {
            onHeavyFail();
        }

        heavyShoveCharge = 0;
        chargeLevel = 0;

        handler.rumble.ForceStopRumble();
        StartCoroutine(ShoveRumbleDelay(.02f));
    }


    /// <summary>
    /// A function called by input handler to activate the heavy shove hitbox
    /// </summary>
    public void EnableShoveBaseHitbox()
    {
        StartCoroutine(HitboxCooldown());
    }

    /// <summary>
    /// Disables the hitbox after some time
    /// </summary>
    /// <returns></returns>
    public IEnumerator HitboxCooldown()
    {
        mover.ForceLockAim();
        collider.enabled = true;
        sr.enabled = true;
        yield return new WaitForSeconds(hitBoxCooldown);
        collider.enabled = false;
        sr.enabled = false;
        mover.ForceUnlockAim();
    }

    /// <summary>
    /// Interrupts charging if hit, takes parameters to match on hit event
    /// </summary>
    public void InterruptChargeOnHit(HitEvent e )
    {
       InterruptCharge();
    }

        /// <summary>
    /// Interrupts charging 
    /// </summary>
    public void InterruptCharge()
    {
        if (heavyShoveIsCharging)
        {
            LoggingInfo.instance.heavyShoveInterrupts[handler.playerConfig.PlayerIndex] += 1;

            handler.rumble.ForceStopRumble();
            heavyShoveIsCharging = false;
            heavyShoveCharge = 0;
            handler.onHeavyShoveComplete();
        }
    }


    public void EnableBaseHeavyShove()
    {
        onHeavyShove += EnableShoveBaseHitbox;
    }

    public void DisableBaseHeavyShove()
    {
        onHeavyShove -= EnableShoveBaseHitbox;
    }

    private IEnumerator ShoveRumbleDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!handler.playerConfig.IsDead)
        {
            handler.rumble.RumbleConstant(1.7f, 2f, .4f);
        }
    }
}

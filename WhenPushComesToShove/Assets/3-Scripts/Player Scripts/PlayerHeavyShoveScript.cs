using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerHeavyShoveScript : MonoBehaviour
{
    public float speedDecrease = .2f;
    public Action onHeavyShove;
    public Action onHeavyCharge;
    public Action onHeavyFail;
    public Hitbox hitbox;
    public float cooldown;
    public float hitBoxCooldown;

    [HideInInspector] public bool heavyShoveIsCharging = false;
    [HideInInspector] public float heavyShoveCharge = 0;
    [HideInInspector] public int chargeLevel = 0;

    [SerializeField] public float lowTierChargeTime = .3f;
    [SerializeField] public float midTierChargeTime = .5f;
    [SerializeField] public float highTierChargeTime = 1;
    [SerializeField] public float lowTierChargeForce = 10000;
    [SerializeField] public float midTierChargeForce = 13500;
    [SerializeField] public float highTierChargeForce = 16000;
    [HideInInspector] public float forceMultiplier = 1.0f;

    private Collider2D collider;
    private PlayerInputHandler handler;
    private PlayerMovementScript mover;

    public void Start()
    {
        collider = hitbox.gameObject.GetComponent<Collider2D>();
        collider.enabled = false;

        mover = GetComponent<PlayerMovementScript>();
        handler = GetComponent<PlayerInputHandler>();

        EnableBaseHeavyShove();

        LevelManager.onNewRoom += InterruptCharge;
    }

    public void Update()
    {
        if (heavyShoveIsCharging)
        {
            heavyShoveCharge += Time.deltaTime * GameState.playerChargeModifier;

            if (heavyShoveCharge >= highTierChargeTime)
            {
                chargeLevel = 3;
                hitbox.knockbackData.strength = highTierChargeForce * forceMultiplier;
            }
            else if (heavyShoveCharge >= midTierChargeTime )
            {
                chargeLevel = 2;
                hitbox.knockbackData.strength = midTierChargeForce * forceMultiplier;
            }
            else if(heavyShoveCharge >= lowTierChargeTime)
            {
                chargeLevel = 1;
                hitbox.knockbackData.strength = lowTierChargeForce * forceMultiplier;
            }
            else
            {
                chargeLevel = 0;
            }
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
        //sr.enabled = true;
        yield return new WaitForSeconds(hitBoxCooldown);
        collider.enabled = false;
        //sr.enabled = false;
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

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
    [SerializeField] public float heavyShoveChargeTime = 1;

    public float speedDecrease = .2f;

    public Action onHeavyShove;
    public Action onHeavyCharge;

    public Hitbox hitbox;
    private Collider2D collider;

    public float cooldown;
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
    }

    public void Update()
    {
        if (heavyShoveIsCharging)
        {
            heavyShoveCharge += Time.deltaTime;
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

            heavyShoveIsCharging = true;
            heavyShoveCharge = 0;
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

        if (heavyShoveCharge >= heavyShoveChargeTime)
        {
            handler.LockAction(cooldown, handler.onHeavyShoveComplete);
            onHeavyShove();
        }

        heavyShoveCharge = 0;
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
        
        collider.enabled = true;
        sr.enabled = true;
        yield return new WaitForSeconds(cooldown);
        collider.enabled = false;
        sr.enabled = false;
    }

    /// <summary>
    /// Interrupts charging if hit
    /// </summary>
    public void InterruptCharge()
    {
        if (heavyShoveIsCharging)
        {
            heavyShoveIsCharging = false;
            heavyShoveCharge = 0;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerLightShoveScript : MonoBehaviour
{
    public Action onLightShove;

    public float speedDecrease = .5f;

    public Hitbox hitbox;
    private Collider2D collider;
    public float cooldown;
    public float hitBoxCooldown = .2f;

    //private SpriteRenderer sr;

    private PlayerMovementScript mover;
    private PlayerInputHandler handler;

    public void Start()
    {
        collider = hitbox.gameObject.GetComponent<Collider2D>();
        collider.enabled = false;
        //sr = hitbox.gameObject.GetComponent<SpriteRenderer>();
        //sr.enabled = false;

        mover = GetComponent<PlayerMovementScript>();
        handler = GetComponent<PlayerInputHandler>();

        EnableBaseLightShove();
    }

    /// <summary>
    /// Triggers the Light Shove
    /// </summary>
    /// <param name="context"></param>
    public void OnLightShoveStart(CallbackContext context)
    {
        if (context.started)
        {
            handler.LockAction(cooldown, handler.onLightShoveComplete);
            StartCoroutine(mover.ChangeMoveSpeedForTime(speedDecrease, cooldown));

            onLightShove();
        }
    }

    /// <summary>
    /// A function called in input handler to activate the hitbox for the light shove
    /// </summary>
    public void EnableShoveBaseHitbox()
    {
        collider.enabled = true;
        //sr.enabled = true;
        mover.ForceLockAim();
        //hitbox.gameObject.SetActive(true);
        StartCoroutine(HitboxCooldown());
    }

    /// <summary>
    /// Disables the hitbox after its cooldown
    /// </summary>
    /// <returns></returns>
    public IEnumerator HitboxCooldown()
    {
        yield return new WaitForSeconds(hitBoxCooldown);
        collider.enabled = false;
        //sr.enabled = false;
        mover.ForceUnlockAim();
    }

    public void EnableBaseLightShove()
    {
        onLightShove += EnableShoveBaseHitbox;
    }

    public void DisableBaseLightShove()
    {
        onLightShove -= EnableShoveBaseHitbox;
    }
}

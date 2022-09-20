using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHeavyShoveScript : MonoBehaviour
{
    public Action onHeavyShove;

    public Hitbox hitbox;
    private Collider2D collider;
    public float cooldown;

    public void Start()
    {
        collider = hitbox.gameObject.GetComponent<Collider2D>();
        collider.enabled = false;

        EnableBaseHeavyShove();
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
        yield return new WaitForSeconds(cooldown);
        collider.enabled = false;
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

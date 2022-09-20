using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHeavyShoveScript : MonoBehaviour
{
    public Action onHeavyShove;

    public Hitbox hitbox;
    private Collider2D collider;
    [SerializeField] private float cooldown = 1f;
    private SpriteRenderer sr;

    public void Start()
    {
        collider = hitbox.gameObject.GetComponent<Collider2D>();
        collider.enabled = false;
        sr = hitbox.gameObject.GetComponent<SpriteRenderer>();
        sr.enabled = false;

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
        sr.enabled = true;
        yield return new WaitForSeconds(cooldown);
        collider.enabled = false;
        sr.enabled = false;
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

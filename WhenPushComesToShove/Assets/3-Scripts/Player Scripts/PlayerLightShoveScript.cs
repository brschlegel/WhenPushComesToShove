using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerLightShoveScript : MonoBehaviour
{
    public Action onLightShove;

    public float speedDecrease = .5f;

    public Hitbox hitbox;
    private Collider2D collider;
    public float cooldown;

    private SpriteRenderer sr;

    public void Start()
    {
        collider = hitbox.gameObject.GetComponent<Collider2D>();
        collider.enabled = false;
        sr = hitbox.gameObject.GetComponent<SpriteRenderer>();
        sr.enabled = false;

        EnableBaseLightShove();
    }

    /// <summary>
    /// A function called in input handler to activate the hitbox for the light shove
    /// </summary>
    public void EnableShoveBaseHitbox()
    {
        collider.enabled = true;
        sr.enabled = true;
        //hitbox.gameObject.SetActive(true);
        StartCoroutine(HitboxCooldown());
    }

    /// <summary>
    /// Disables the hitbox after its cooldown
    /// </summary>
    /// <returns></returns>
    public IEnumerator HitboxCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        collider.enabled = false;
        sr.enabled = false;
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

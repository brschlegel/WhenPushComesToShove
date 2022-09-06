using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerLightShoveScript : MonoBehaviour
{
    public Action onLightShove;

    [SerializeField] private GameObject hitbox;
    [SerializeField] private float cooldown = .5f;

    public void Start()
    {
        hitbox.SetActive(false);

        EnableBaseLightShove();
    }

    /// <summary>
    /// A function called in input handler to activate the hitbox for the light shove
    /// </summary>
    public void EnableShoveBaseHitbox()
    {
        hitbox.SetActive(true);
        StartCoroutine(HitboxCooldown());
    }

    /// <summary>
    /// Disables the hitbox after its cooldown
    /// </summary>
    /// <returns></returns>
    public IEnumerator HitboxCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        hitbox.SetActive(false);
    }

    public void EnableBaseLightShove()
    {
        onLightShove += EnableShoveBaseHitbox;
    }

    public void DisableBaseLightShove()
    {
        onLightShove = null;
    }
}

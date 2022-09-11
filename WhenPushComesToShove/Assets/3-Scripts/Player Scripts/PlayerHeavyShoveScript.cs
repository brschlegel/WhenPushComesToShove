using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHeavyShoveScript : MonoBehaviour
{
    public Action onHeavyShove;

    public Hitbox hitbox;
    [SerializeField] private float cooldown = .5f;

    public void Start()
    {
        hitbox.gameObject.SetActive(false);

        EnableBaseHeavyShove();
    }

    /// <summary>
    /// A function called by input handler to activate the heavy shove hitbox
    /// </summary>
    public void EnableShoveBaseHitbox()
    {
        hitbox.gameObject.SetActive(true);
        StartCoroutine(HitboxCooldown());
    }

    /// <summary>
    /// Disables the hitbox after some time
    /// </summary>
    /// <returns></returns>
    public IEnumerator HitboxCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        hitbox.gameObject.SetActive(false);
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

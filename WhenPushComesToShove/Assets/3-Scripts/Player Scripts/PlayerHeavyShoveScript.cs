using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyShoveScript : MonoBehaviour
{
    [SerializeField] private GameObject hitbox;
    [SerializeField] private float cooldown = .5f;

    public void Start()
    {
        hitbox.SetActive(false);
    }

    /// <summary>
    /// A function called by input handler to activate the heavy shove hitbox
    /// </summary>
    public void OnHeavyShove()
    {
        hitbox.SetActive(true);
        StartCoroutine(HitboxCooldown());
    }

    /// <summary>
    /// Disables the hitbox after some time
    /// </summary>
    /// <returns></returns>
    public IEnumerator HitboxCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        hitbox.SetActive(false);
    }
}

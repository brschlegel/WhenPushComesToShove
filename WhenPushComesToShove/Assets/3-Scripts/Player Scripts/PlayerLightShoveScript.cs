using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightShoveScript : MonoBehaviour
{
    [SerializeField] private GameObject hitbox;
    [SerializeField] private float cooldown = .5f;

    public void Start()
    {
        hitbox.SetActive(false);
    }

    /// <summary>
    /// A function called in input handler to activate the hitbox for the light shove
    /// </summary>
    public void OnLightShove()
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
}

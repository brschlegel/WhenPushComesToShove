using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WallData : MonoBehaviour
{
    //[Tooltip("How much an object will bounce off of this wall (0 for no bounce, 1 for no speed lost")]
    //[Range(0,1)]
    //public float elasticity;
    //[Tooltip("How much to multiply the velocity-based damage multiplier")]
    //public float damageMultiplier;

    public float damage = 5f;
    public float velocityDamageMultiplier = 1f;
    public float invulnerabilityCooldown = .5f;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Check for health component
        Health  health = collision.gameObject.GetComponentInChildren<Health>();

        //Cheack for Projectile Mode
        ProjectileMode pMode = collision.gameObject.GetComponentInChildren<ProjectileMode>();

        if (health != null && pMode != null)
        {
            if (pMode.enabled && !health.invulnerable && ShouldTakeDamage(pMode.sourceObject, collision.gameObject))
            {
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

                Debug.Log("Hit Wall");
                health.WallInvulnerabilityCooldown(invulnerabilityCooldown);
                health.TakeDamage(damage + (rb.velocity.magnitude * velocityDamageMultiplier), pMode.sourceObject.name);
            }
        }
    }

    private bool ShouldTakeDamage(GameObject source, GameObject collision)
    {
        //if (GameState.currentRoomType != LevelType.Arena)//Except Pvp
        //{
        //    Debug.Log("No Damage");
        //    return false;
        //}
        //else if (source.tag == "Hazard")
        //{
        //    return false;
        //}
        //
        //return true;

        return false;
    }
}

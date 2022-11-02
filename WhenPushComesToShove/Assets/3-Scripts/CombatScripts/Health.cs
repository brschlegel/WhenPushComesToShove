using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    protected float currentHealth;
    public float maxHealth;
    [HideInInspector] public bool dead;
    [HideInInspector] public bool invulnerable = false;


    public float CurrentHealth
    {
        get { return currentHealth;}
    }
    public virtual void TakeDamage(float damage, string source)
    {
        if (!dead)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die(source);
            }
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    public virtual void Die(string source)
    {
        currentHealth = maxHealth;
        dead = true;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        dead = false;
    }

    public IEnumerator WallInvulnerabilityCooldown(float cooldown)
    {
        invulnerable = true;

        yield return new WaitForSeconds(cooldown);

        invulnerable = false;
    }
}

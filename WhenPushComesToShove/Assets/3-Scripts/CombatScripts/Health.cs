using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    protected float currentHealth;
    public float maxHealth;
    [HideInInspector] public bool dead;


    public float CurrentHealth
    {
        get { return currentHealth;}
    }
    public virtual void TakeDamage(float damage, string source)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die(source);
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
}

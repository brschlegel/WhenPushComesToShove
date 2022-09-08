using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float currentHealth;
    public float maxHealth;


    public float CurrentHealth
    {
        get { return currentHealth;}
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    public void Die()
    {
        Debug.Log(transform.root.name + " is dead!");
        currentHealth = maxHealth;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }
}

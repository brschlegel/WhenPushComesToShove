using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EnemyHealth : Health
{
    
    [SerializeField]
    private GameObject rootObject; 

    public UnityEvent onDeath;
    public override void Die(string source)
    {
        onDeath?.Invoke();
    }
}

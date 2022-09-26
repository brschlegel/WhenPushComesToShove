using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Hitstun : MonoBehaviour
{

    [SerializeField]
    private ProjectileMode pMode;

    /// <summary>
    /// Place entity in hitstun
    /// </summary>
    protected abstract void Stun();

    /// <summary>
    /// Get entity out of hitstun
    /// </summary>
    protected abstract void Unstun();

    private void OnEnable()
    {
        Stun();
    }

    private void OnDisable()
    {
        Unstun();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public delegate void DashEvent (Vector3 v);
public class PlayerDashScript : MonoBehaviour
{
    [HideInInspector] public ProjectileMode pMode;
    [SerializeField] private float dashSpeed = 15;
    [SerializeField] private float dashTime = 1;

    public event DashEvent onDashStart;
    /// <summary>
    /// A function called in input handler to trigger a dash
    /// </summary>
    /// <param name="playerIndex"></param>
    public void OnDash(Vector3 dashDirection)
    {
        if (pMode != null)
        {
            pMode.AddForce(dashDirection * dashSpeed);
            onDashStart?.Invoke(dashDirection);
        }
    }
}

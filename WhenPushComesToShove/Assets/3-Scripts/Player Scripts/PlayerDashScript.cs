using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashScript : MonoBehaviour
{
    [HideInInspector] public VelocitySetter vs;
    [SerializeField] private float dashSpeed = 15;
    [SerializeField] private float dashTime = 1;

    /// <summary>
    /// A function called in input handler to trigger a dash
    /// </summary>
    /// <param name="playerIndex"></param>
    public void OnDash(Vector3 dashDirection)
    {
        if (vs != null)
        {
            vs.AddSourceForTime("playerDash", dashDirection * dashSpeed, dashTime);
        }
    }
}

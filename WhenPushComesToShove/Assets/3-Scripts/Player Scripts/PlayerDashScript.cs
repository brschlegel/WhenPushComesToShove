using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashScript : MonoBehaviour
{
    /// <summary>
    /// A function called in input handler to trigger a dash
    /// </summary>
    /// <param name="playerIndex"></param>
    public void OnDash(int playerIndex)
    {
        Debug.Log("Player " + playerIndex + " Dash");
    }
}

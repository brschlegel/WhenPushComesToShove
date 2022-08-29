using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashScript : MonoBehaviour
{
    public void OnDash(int playerIndex)
    {
        Debug.Log("Player " + playerIndex + " Dash");
    }
}

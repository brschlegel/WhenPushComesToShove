using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightShoveScript : MonoBehaviour
{
    public void OnLightShove( int playerIndex )
    {
        Debug.Log("Player " + playerIndex + " Light Shove");
    }
}

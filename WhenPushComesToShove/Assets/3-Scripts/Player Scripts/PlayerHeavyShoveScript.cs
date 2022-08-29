using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyShoveScript : MonoBehaviour
{
    public void OnHeavyShove(int playerIndex)
    {
        Debug.Log("Player " + playerIndex + " Heavy Shove");
    }
}

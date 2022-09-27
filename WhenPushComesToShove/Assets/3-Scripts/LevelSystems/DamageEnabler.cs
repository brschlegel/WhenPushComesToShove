using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnabler : MonoBehaviour
{

    public void EnableDamage(bool enable)
    {
        foreach(Transform t in GameState.players)
        {
            t.GetComponentInChildren<DamageHitHandler>().damageEnabled = enable;
        }
    } 
}

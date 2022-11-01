using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnabler : MonoBehaviour
{

    public void EnableDamage(bool enable)
    {
       GameState.damageEnabled = true;
    } 
}

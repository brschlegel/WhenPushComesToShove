using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KnockbackData : MonoBehaviour
{
    public KnockbackType type;
    public float strength;
    public abstract Vector2 GetDirection(HitEvent e);
    
}

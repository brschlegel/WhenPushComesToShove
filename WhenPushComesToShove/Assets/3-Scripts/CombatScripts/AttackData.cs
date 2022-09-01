using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KnockbackType{Normal, None}
//Data class regarding attacks
public class AttackData : MonoBehaviour
{
   public float strength;
   public float force;
   public KnockbackType type;
}

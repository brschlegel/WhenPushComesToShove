using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WallData : MonoBehaviour
{
    [Tooltip("How much an object will bounce off of this wall (0 for no bounce, 1 for no speed lost")]
    [Range(0,1)]
    public float elasticity;
}

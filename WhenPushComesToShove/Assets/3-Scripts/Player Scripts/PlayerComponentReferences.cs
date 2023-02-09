using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A class to hold references to components for each player
public class PlayerComponentReferences : MonoBehaviour
{
    public SpriteRenderer teamIcon;
    public SpriteRenderer crownIcon;
    public SpriteRenderer haloIcon;
    public SpriteRenderer sword;

    public Transform GroundUIRef;

    public ParticleSystem circleVFX;
}

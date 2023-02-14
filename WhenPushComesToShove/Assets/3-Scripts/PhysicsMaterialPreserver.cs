using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//We change up the values on our physics material with a modifier. This change is a permenant file change, so we try to be diligent about changing it back, but crashes and the like 
//can lead to permanent changes. So just copy it over at the start of the game 
public class PhysicsMaterialPreserver : MonoBehaviour
{
    [SerializeField]
    private PhysicsMaterial2D source;
    [SerializeField]
    private PhysicsMaterial2D usedMaterial;

    private void Awake()
    {
        usedMaterial.bounciness = source.bounciness;
        usedMaterial.friction = source.friction;
    }
}

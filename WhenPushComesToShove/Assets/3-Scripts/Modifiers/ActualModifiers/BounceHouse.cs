using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceHouse : BaseModifier
{

    [SerializeField]
    private float bouncinessMult;
    [SerializeField]
    private PhysicsMaterial2D material;

    public override void Init()
    {
        material.bounciness *= bouncinessMult;
    }

    public override void CleanUp()
    {
        material.bounciness /= bouncinessMult;
    }

    //Turns out edits to physics materials in code are permanent! Like persisting after the game is done running permanent. So make sure we are always cleaning up
    void OnDestroy()
    {
        CleanUp();
    }
}

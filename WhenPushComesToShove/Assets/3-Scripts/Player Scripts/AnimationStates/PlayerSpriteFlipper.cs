using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSpriteFlipper : MonoBehaviour
{
    [Tooltip("To face transform.right, should the sprite be flipped?")]
    [SerializeField]
    private bool isRightFlipped;
    [SerializeField]
    private VelocitySetter vs;
    [SerializeField]
    private Transform aimDirectionObject;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //aim direction based 
        bool isXPositive = aimDirectionObject.right.x >= 0;
        // (isRightFlipped && isXPositive) || (!isRightFlipped && !isXPositive);
        sr.flipX = isRightFlipped == isXPositive;

        //velocity based
        // if(vs.QuerySource("playerDash", out Vector2 dash))
        // {
        //     if(dash.magnitude >= 0)
        //     {
        //         bool isXPositive = dash.x >= 0;
        //         // (isRightFlipped && isXPositive) || (!isRightFlipped && !isXPositive);
        //         sr.flipX = isRightFlipped == isXPositive;
        //     }
        // }
        // if(vs.QuerySource("playerMovement", out Vector2 movement))
        // {
        //     //Don't change anything if we aren't moving in the x
        //     if(movement.x == 0)
        //     {
        //         return;
        //     }
        //     bool isXPositive = movement.x > 0;
        //     // (isRightFlipped && isXPositive) || (!isRightFlipped && !isXPositive);
        //     sr.flipX = isRightFlipped == isXPositive;
        // }
    }
}

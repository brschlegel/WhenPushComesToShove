using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionSlimeAttackState : State
{
    [HideInInspector]
    public SpriteRenderer sprite;
    [HideInInspector]
    public Vector2 target;

    private void OnEnable()
    {
        anim.Play("Slime_Jump");
    }
    // Update is called once per frame
    void Update()
    {
        sprite.flipX = !((target - (Vector2)transform.position).x > 0);
        transform.position = Vector2.MoveTowards(transform.position, target,2 *Time.deltaTime );
        if(Vector2.Distance(target, (Vector2)transform.position) <= .1f)
        {
            enabled = false;
            InvokeOnStateExit(true);
        }
    }
}

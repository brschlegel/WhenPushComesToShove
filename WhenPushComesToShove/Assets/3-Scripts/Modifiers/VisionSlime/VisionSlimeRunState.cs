using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionSlimeRunState : State
{
   [HideInInspector]
   public Vector2 target;
   [HideInInspector]
   public SpriteRenderer sprite;

   private float speed = 2;
 

   private void OnEnable()
   {
     anim.Play("Slime_Run", 0);
   }

   private void Update()
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBrain : MonoBehaviour
{
    [SerializeField]
    private Chase chase;
    [SerializeField]
    private JumpAttack jumpAttack;
    [SerializeField]
    private VelocitySetter vs;
    [SerializeField]
    private EnemyHitstun hitstun;
    [SerializeField]
    private MovementController movement;
    private Transform target;

    private bool isAttacking;
    // Start is called before the first frame update
    void Start()
    {
        jumpAttack.vs = vs;
        jumpAttack.onAttackEnd += JumpAttackEnd;
        jumpAttack.movement = movement;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            target = PickTarget();
            if (target != null)
            {
                chase.SetTarget(target);
            }
        }
        else if(chase.closeEnough && !isAttacking && !hitstun.inHitstun)
        {
            JumpAttack();
        }
    }

    private Transform PickTarget()
    {
        float dist = float.PositiveInfinity;
        Transform closest = null;
        foreach(Transform t in GameState.players)
        {
            float temp = Vector2.Distance(transform.position, t.position);
            if(temp < dist)
            {
                dist = temp;
                closest = t;
            }
        }
        return closest;
    }

    public void JumpAttack()
    {

        isAttacking = true;
        jumpAttack.Attack(target);
    }

    public void JumpAttackEnd(Transform target)
    {
        isAttacking = false;
    }
}

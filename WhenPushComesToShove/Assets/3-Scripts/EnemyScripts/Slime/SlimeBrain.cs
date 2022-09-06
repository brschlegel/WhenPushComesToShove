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
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        jumpAttack.vs = vs;
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
                chase.enabled = true;
            }
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
        jumpAttack.Attack(target);
    }
}

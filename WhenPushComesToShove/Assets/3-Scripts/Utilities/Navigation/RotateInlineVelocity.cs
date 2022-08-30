using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis {XAxis, YAxis}
[RequireComponent(typeof(VelocitySetter))]
public class RotateInlineVelocity : MonoBehaviour
{
    public Axis axis;
    private VelocitySetter vs;
    // Start is called before the first frame update
    void Start()
    {
        vs = GetComponent<VelocitySetter>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(vs.Velocity != Vector2.zero)
        {
            if (axis == Axis.XAxis)
            {
                transform.right = vs.Velocity;
            }
            else
            {
                transform.up = vs.Velocity;
            }
        }
    }
}

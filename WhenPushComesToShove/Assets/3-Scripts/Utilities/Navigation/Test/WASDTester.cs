using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VelocitySetter))]
public class WASDTester : MonoBehaviour
{
    VelocitySetter vs;
    public float speed = 5;
     public Vector2 inputVector;
    // Start is called before the first frame update
    void Start()
    {
        vs = GetComponent<VelocitySetter>();
    }

    void FixedUpdate()
    {
       inputVector = Vector2.zero;
        if(Input.GetKey(KeyCode.W))
        {
            inputVector += Vector2.up;
            
        }
        if(Input.GetKey(KeyCode.S))
        {
            inputVector += -Vector2.up;
            
        }
        if(Input.GetKey(KeyCode.D))
        {
            inputVector += Vector2.right;
            
        }
        if(Input.GetKey(KeyCode.A))
        {
            inputVector += -Vector2.right;
            
        }

        vs.AddSource("Input", inputVector, speed);
       
    }
}

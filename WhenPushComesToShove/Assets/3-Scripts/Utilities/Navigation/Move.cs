using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(VelocitySetter))]
public abstract class Move : MonoBehaviour
{
    public float speed;
    protected VelocitySetter velocitySetter;
    [HideInInspector]
    public Vector2 target;

    protected void Start()
    {
        if(velocitySetter == null)
        {
            Init();
        }
    }

    public void Init()
    {
        velocitySetter = GetComponent<VelocitySetter>();
    }

    public abstract Vector3 GetMovementDirection(Vector2 target);

    public abstract void Stop();
  

}

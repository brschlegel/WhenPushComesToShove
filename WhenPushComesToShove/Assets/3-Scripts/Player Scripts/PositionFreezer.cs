using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PositionFreezer : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 lockedPosition;
    // Start is called before the first frame update
    void Start()
    {
       // this.enabled = false;
    }

    public void LockPosition(Vector2 position)
    {
        lockedPosition = position;
        this.enabled = true;
    }

    public void UnlockPosition()
    {
        this.enabled = false;
    }

    private void Update()
    {
        Rigidbody.position = lockedPosition;
        Rigidbody.velocity = Vector2.zero;
    }

    //Use property to initalize itself
    private Rigidbody2D Rigidbody
    {
        get 
        {
            if(rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }
            return rb;
        }
    }


}

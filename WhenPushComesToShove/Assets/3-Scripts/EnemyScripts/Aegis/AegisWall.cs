using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AegisWall : MonoBehaviour
{
    [HideInInspector]
    public Transform target;
    public Transform wallTransform;
    public float offset;
    // Start is called before the first frame update
    void Start()
    {
        wallTransform = transform.GetChild(0);
        ResetWallPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.right = target.position - transform.position ;
        }
 

    }

    public void ResetWallPosition()
    {
        wallTransform.localPosition = new Vector3(offset, 0 ,0);
    }
}

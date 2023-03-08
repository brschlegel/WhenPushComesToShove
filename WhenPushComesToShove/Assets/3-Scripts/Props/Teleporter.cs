using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private Vector2 destination;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Ensures the object is teleporting along the z axis
            collision.transform.position = new Vector3(destination.x, destination.y, collision.transform.position.z);
        }        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(destination, 0.3f);
        Gizmos.DrawLine(gameObject.transform.position, destination);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//A bit of a hack to keep people and objects out of the walls
[RequireComponent(typeof(Collider2D))]
public class WallEnforcer : MonoBehaviour
{
    [SerializeField]
    private List<string> tagsToTeleport;
    [SerializeField]
    private Transform teleportLocation;
    public void OnTriggerEnter2D(Collider2D collider)
    {
       
        if (tagsToTeleport.Contains(collider.gameObject.tag))
        {
            collider.attachedRigidbody.transform.position = teleportLocation.position;
            
        }
    }
}

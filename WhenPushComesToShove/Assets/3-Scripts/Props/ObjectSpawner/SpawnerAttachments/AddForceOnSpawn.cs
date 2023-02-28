using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectSpawner))]
public class AddForceOnSpawn : SpawnerAttachment
{
    public Vector2 force;
    protected override void OnSpawn(Transform t)
    {
        Rigidbody2D rb = t.GetComponent<Rigidbody2D>();
        rb.AddForce(force);
    }
}

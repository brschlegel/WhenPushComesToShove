using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectSpawner))]
public class SpawnWithVelocity : SpawnerAttachment
{
    public Vector2 velocityDirection;
    public float initialForce;

    protected override void OnSpawn(Transform t)
    {
        ConstantForce2D con = t.GetComponent<ConstantForce2D>();

        //Vector2 force = velocityDirection.normalized * initialForce;
        //rb.AddForce(force);
        con.force = velocityDirection.normalized * initialForce;
    }
}

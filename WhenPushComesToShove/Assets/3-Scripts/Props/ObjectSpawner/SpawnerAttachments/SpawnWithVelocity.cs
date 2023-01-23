using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectSpawner))]
public class SpawnWithVelocity : SpawnerAttachment
{
    [SerializeField] Vector2 velocityDirection;
    [SerializeField] float initialForce;

    protected override void OnSpawn(Transform t)
    {
        ConstantForce2D con = t.GetComponent<ConstantForce2D>();

        //Vector2 force = velocityDirection.normalized * initialForce;
        //rb.AddForce(force);
        con.force = velocityDirection.normalized * initialForce;
    }
}

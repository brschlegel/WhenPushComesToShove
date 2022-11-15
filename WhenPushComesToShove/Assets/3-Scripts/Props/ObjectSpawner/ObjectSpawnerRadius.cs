using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerRadius : SingleObjectSpawner
{
    public float radius;
    public override Vector2 GetSpawnLocation()
    {
        return (Vector2)transform.position + Random.insideUnitCircle * radius;
    }
}

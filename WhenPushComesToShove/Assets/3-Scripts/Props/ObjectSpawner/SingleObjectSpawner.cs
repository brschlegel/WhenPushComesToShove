using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleObjectSpawner : ObjectSpawner
{
    public abstract Vector2 GetSpawnLocation();

    public override void Spawn()
    {
        if (spawnedObjectParent.childCount <= numSpawnedLimit)
        {
            Transform t = Instantiate(objectToSpawn, GetSpawnLocation(), Quaternion.identity, spawnedObjectParent).transform;
            InvokeOnObjectSpawned(t);
        }
    }
}

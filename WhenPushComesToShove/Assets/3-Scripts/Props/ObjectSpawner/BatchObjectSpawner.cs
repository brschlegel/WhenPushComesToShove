using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BatchObjectSpawner : ObjectSpawner
{
    public override void Spawn()
    {
        if (spawnedObjectParent.childCount <= numSpawnedLimit)
        {
            List<Vector2> locations = GetBatchSpawnLocations();
            foreach (Vector2 v in locations)
            {
                Transform t = Instantiate(objectToSpawn, v, Quaternion.identity, spawnedObjectParent).transform;
                InvokeOnObjectSpawned(t);
            }
        }
    }

    public abstract List<Vector2> GetBatchSpawnLocations();
}

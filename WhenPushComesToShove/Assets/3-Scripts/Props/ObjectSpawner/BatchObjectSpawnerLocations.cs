using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatchObjectSpawnerLocations : BatchObjectSpawner
{
    public override List<Vector2> GetBatchSpawnLocations()
    {
        List<Vector2> locations = new List<Vector2>();
        foreach(Transform child in transform)
        {
            locations.Add(child.position);
        }
        return locations;
    }
}

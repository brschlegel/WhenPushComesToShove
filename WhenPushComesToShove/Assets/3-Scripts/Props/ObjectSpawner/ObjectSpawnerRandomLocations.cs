using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerRandomLocations : SingleObjectSpawner
{

    //Children are the list of locations
    public override Vector2 GetSpawnLocation()
    {
        return transform.GetChild(Random.Range(0,transform.childCount)).position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleObjectSpawner : ObjectSpawner
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract Vector2 GetSpawnLocation();

    public override void Spawn()
    {
        Transform t = Instantiate(objectToSpawn, GetSpawnLocation(), Quaternion.identity, spawnedObjectParent).transform;
        InvokeOnObjectSpawned(t);
    }
}

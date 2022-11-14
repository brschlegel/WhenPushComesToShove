using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectSpawner))]
public class SpawnOnExplode : SpawnerAttachment
{

    protected override void OnSpawn(Transform t)
    {
        Explosion e = t.GetComponentInChildren<Explosion>();
        e.onExplode += spawner.SpawnWithDelay;
    }
    
}

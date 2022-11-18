using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectSpawner))]
public class RandomizeCountdownOnSpawn : SpawnerAttachment
{

    public int min = 2;
    public int max = 8;
    protected override void OnSpawn(Transform t)
    {
        int rand = Random.Range(min, max);
        ExplosionTimerFlash flash = t.GetComponentInChildren<ExplosionTimerFlash>();
        if(flash != null)
        {
            flash.maxTime = rand;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectSpawner))]
public class SetExplodeTimeOnSpawn : SpawnerAttachment
{
    public float time;
    protected override void OnSpawn(Transform t)
    {
        ExplosionTimerFlash timer = t.GetComponentInChildren<ExplosionTimerFlash>();
        if (timer != null)
        {
            timer.maxTime = time;
        }
    }
}

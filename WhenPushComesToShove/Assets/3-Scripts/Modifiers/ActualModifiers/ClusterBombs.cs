using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterBombs : EventModifier
{

  

    [SerializeField]
    private GameObject clusterBomb;
    [Header("Spawn Parameters")]
    [SerializeField]
    private int numberOfBombs;
    [SerializeField]
    private float spawnForce;
    [SerializeField]
    private float timeToExplode;

    public override void Init()
    {
        key = "BombExploded";
        base.Init();
    }

    protected override void OnEvent(MessageArgs args)
    {
        List<Vector2> points = BenMath.GetEquidistantPoints((Vector2)args.vectorArg, 10, numberOfBombs, Mathf.PI / 2);
        for(int i = 0; i < numberOfBombs;  i++)
        {
            GameObject g = Instantiate(clusterBomb,(Vector2)args.vectorArg , Quaternion.identity, ((GameObject)args.objectArg).transform.parent);
            Vector2 dir = points[i] - (Vector2)args.vectorArg;
            ProjectileMode pMode = g.GetComponentInChildren<ProjectileMode>();
            pMode.enabled = true;
            pMode.AddForce(spawnForce * dir.normalized);

            ExplosionTimerFlash e = g.GetComponentInChildren<ExplosionTimerFlash>();
            e.MaxTime = timeToExplode;
        }
    }



}

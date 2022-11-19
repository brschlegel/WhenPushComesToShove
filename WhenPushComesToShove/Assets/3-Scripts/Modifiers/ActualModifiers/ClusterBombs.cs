using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterBombs : BaseModifier
{
    uint eventID;

    [SerializeField]
    private GameObject clusterBomb;
    [Header("Spawn Parameters")]
    [SerializeField]
    private int numberOfBombs;
    [SerializeField]
    private float spawnForce;
    [SerializeField]
    private float timeToExplode;


    // Start is called before the first frame update
    void Start()
    {
        eventID = Messenger.RegisterEvent("BombExploded", OnBombExplode);
    }

    private void OnBombExplode(MessageArgs args)
    {
        List<Vector2> points = BenMath.GetEquidistantPoints((Vector2)args.vectorArg, 10, numberOfBombs, Mathf.PI / 2);
        for(int i = 0; i < numberOfBombs;  i++)
        {
            GameObject g = Instantiate(clusterBomb,(Vector2)args.vectorArg , Quaternion.identity, currentGameRoot);
            Vector2 dir = points[i] - (Vector2)args.vectorArg;
            ProjectileMode pMode = g.GetComponentInChildren<ProjectileMode>();
            pMode.enabled = true;
            pMode.AddForce(spawnForce * dir.normalized);

            ExplosionTimerFlash e = g.GetComponentInChildren<ExplosionTimerFlash>();
            e.maxTime = timeToExplode;
        }
    }

    public override void CleanUp()
    {
        Messenger.UnregisterEvent("BombExploded", eventID);
    }

}

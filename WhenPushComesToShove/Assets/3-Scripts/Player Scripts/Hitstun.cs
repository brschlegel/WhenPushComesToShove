using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Hitstun : MonoBehaviour
{

    [SerializeField]
    private VelocitySetter vs;
    [SerializeField]
    private float hitstunThreshold;
    public bool inHitstun;
    protected List<string> sourcesToIgnore;

    private void Update()
    {

        float ignoreMagnitude = 0;
        foreach (string source in sourcesToIgnore)
        {
            if (vs.QuerySource(source, out Vector2 vel))
            {
                ignoreMagnitude += vel.magnitude;
            }
        }

        if (vs.ListedVelocity.magnitude - ignoreMagnitude >= hitstunThreshold)
        {
            inHitstun = true;
            Stun();
        }
        else
        {
            inHitstun = false;
            Unstun();
        }
    }

    /// <summary>
    /// Place entity in hitstun
    /// </summary>
    protected abstract void Stun();

    /// <summary>
    /// Get entity out of hitstun
    /// </summary>
    protected abstract void Unstun();
}

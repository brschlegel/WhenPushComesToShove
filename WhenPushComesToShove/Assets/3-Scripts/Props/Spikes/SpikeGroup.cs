using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeGroup : MonoBehaviour
{
    [HideInInspector]
    public SpikeState groupState;

    private List<Spikes> spikes;
    // Start is called before the first frame update
    void Start()
    {
        spikes = new List<Spikes>(GetComponentsInChildren<Spikes>());
        groupState = spikes[0].state;
    }

    public void Activate()
    {
        foreach(Spikes s in spikes)
        {
            s.Activate();
        }
        groupState = SpikeState.On;
    }

    public void Threaten()
    {
        foreach(Spikes s in spikes)
        {
            s.Threaten();
        }
        groupState = SpikeState.Threatening;
    }

    public void Deactivate()
    {
        foreach(Spikes s in spikes)
        {
            s.Deactivate();
        }

        groupState = SpikeState.Off;
    }
}

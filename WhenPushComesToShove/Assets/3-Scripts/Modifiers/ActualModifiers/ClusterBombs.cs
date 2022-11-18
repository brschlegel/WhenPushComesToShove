using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterBombs : BaseModifier
{
    uint eventID;
    // Start is called before the first frame update
    void Start()
    {
        eventID = Messenger.RegisterEvent("BombExploded", OnBombExplode);
    }

    private void OnBombExplode(MessageArgs args)
    {
        Debug.Log("Bomb " + ((GameObject)args.objectArg).name + " has exploded at " + args.vectorArg);
    }

    public override void CleanUp()
    {
        Messenger.UnregisterEvent("BombExploded", eventID);
    }

}

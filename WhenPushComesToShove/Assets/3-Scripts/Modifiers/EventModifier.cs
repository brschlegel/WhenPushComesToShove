using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventModifier : BaseModifier
{
    protected uint eventID;
    protected string key;

    public override void Init()
    {
        eventID = Messenger.RegisterEvent(key, OnEvent);
    }

    protected abstract void OnEvent(MessageArgs args);

    public override void CleanUp()
    {
        Messenger.UnregisterEvent(key, eventID);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventModifier : BaseModifier
{
    //Serializing these values are required to allow the modifiers to be cleaned up
    [SerializeField]
    [HideInInspector]
    protected uint eventID;
    [SerializeField]
    [HideInInspector]
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

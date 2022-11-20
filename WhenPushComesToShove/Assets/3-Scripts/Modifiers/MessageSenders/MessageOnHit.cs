using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageOnHit : HitHandler
{

    [Tooltip("The key for the message sent will be  keyStart_Hit")]
    public string keyStart;
    public override void ReceiveHit(HitEvent e)
    {
        Messenger.SendEvent(keyStart + "_Hit", new MessageArgs(objectArg: e));
    }
}

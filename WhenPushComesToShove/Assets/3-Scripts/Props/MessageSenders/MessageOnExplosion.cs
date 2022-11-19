using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explosion))]
public class MessageOnExplosion : MonoBehaviour
{
    Explosion exp;
    private void Start()
    {
        exp = GetComponent<Explosion>();
        exp.onExplode += OnExplosion;
    }

    private void OnExplosion()
    {
        Messenger.SendEvent("BombExploded", new MessageArgs(objectArg: exp, vectorArg: exp.gameObject.transform.position));
    }
}

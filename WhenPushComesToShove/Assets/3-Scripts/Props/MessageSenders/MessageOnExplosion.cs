using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explosion))]
public class MessageOnExplosion : MonoBehaviour
{
    [SerializeField]
    private string key;
    Explosion exp;
    private void Start()
    {
        exp = GetComponent<Explosion>();
        exp.onExplode += OnExplosion;
    }

    private void OnExplosion()
    {
        Messenger.SendEvent(key, new MessageArgs(objectArg: exp.rootObject, vectorArg: exp.gameObject.transform.position));
    }
}

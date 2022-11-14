using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectSpawner))]
public abstract class SpawnerAttachment : MonoBehaviour
{
    protected ObjectSpawner spawner;

    void Start()
    {
        if(spawner == null)
        {
            Init();
        }
    }

    public virtual void Init()
    {
       spawner = GetComponent<ObjectSpawner>();
       spawner.onObjectSpawned += OnSpawn;
    }

    protected abstract void OnSpawn(Transform t);
}

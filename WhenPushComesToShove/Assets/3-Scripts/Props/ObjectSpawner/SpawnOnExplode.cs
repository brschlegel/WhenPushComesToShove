using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectSpawner))]
public class SpawnOnExplode : MonoBehaviour
{

    private ObjectSpawner spawner;
    // Start is called before the first frame update
    void Start()
    {
        if(spawner == null)
        {
            Init();
        }
    }

    private void OnSpawn(Transform t)
    {
        Explosion e = t.GetComponentInChildren<Explosion>();
        e.onExplode += spawner.SpawnWithDelay;
    }
    public void Init()
    {
       spawner = GetComponent<ObjectSpawner>();
       spawner.onObjectSpawned += OnSpawn;
    }
}

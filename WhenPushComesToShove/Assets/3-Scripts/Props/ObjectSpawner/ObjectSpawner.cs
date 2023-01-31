using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ObjectSpawned(Transform t);
public abstract class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    protected GameObject objectToSpawn;
    public Transform spawnedObjectParent;
    [SerializeField]
    private bool repeating;
    [SerializeField]
    protected int numSpawnedLimit = 100;

    public event ObjectSpawned onObjectSpawned;
    public float spawnDelay;
    void Start()
    {

    }

    public void InvokeOnObjectSpawned(Transform t)
    {
        onObjectSpawned?.Invoke(t);
    }

    public abstract void Spawn();

    private IEnumerator CoroutineSpawnWithDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        Spawn();
        if (repeating)
        {
            SpawnWithDelay();
        }

    }

    public void SpawnWithDelay()
    {
        CoroutineManager.StartGlobalCoroutine(CoroutineSpawnWithDelay());
    }

    public void SpawnWithoutDelay()
    {
        Spawn();
    }

    public GameObject ObjectToSpawn
    {
        get { return objectToSpawn; }
        set { objectToSpawn = value; }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Spawn();
        }
    }

    public void CleanUpSpawnedObjects()
    {
        for (int i = spawnedObjectParent.childCount - 1; i >= 0; i--)
        {
            Destroy(spawnedObjectParent.GetChild(i).gameObject);
        }
    }
}

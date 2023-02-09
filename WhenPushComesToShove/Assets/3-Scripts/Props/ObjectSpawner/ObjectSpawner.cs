using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ObjectSpawned(Transform t);
public abstract class ObjectSpawner : MonoBehaviour
{
    public Transform spawnedObjectParent;
    public event ObjectSpawned onObjectSpawned;
    public float spawnDelay;
       
    [SerializeField]
    protected int numSpawnedLimit = 100;
    [SerializeField]
    protected GameObject objectToSpawn;

    [SerializeField]
    private bool repeating;

    public GameObject ObjectToSpawn
    {
        get { return objectToSpawn; }
        set { objectToSpawn = value; }
    }


    public abstract void Spawn();

    /// <summary>
    /// Invokes the onObjectSpawned event (Children cannot directly invoke the event)
    /// </summary>
    /// <param name="t">Transform</param>
    protected void InvokeOnObjectSpawned(Transform t)
    {
        onObjectSpawned?.Invoke(t);
    }

    /// <summary>
    /// Calls the spawn method with the spawn delay variable
    /// </summary>
    public void SpawnWithDelay()
    {
        CoroutineManager.StartGlobalCoroutine(CoroutineSpawnWithDelay());
    }

    /// <summary>
    /// Calls the spawn method without any delay
    /// </summary>
    public void SpawnWithoutDelay()
    {
        Spawn();
    }

    /// <summary>
    /// Deletes all spawned objects
    /// </summary>
    public void CleanUpSpawnedObjects()
    {
        for (int i = spawnedObjectParent.childCount - 1; i >= 0; i--)
        {
            Destroy(spawnedObjectParent.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Spawn();
        }
    }

    private IEnumerator CoroutineSpawnWithDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        Spawn();
        if (repeating)
        {
            SpawnWithDelay();
        }

    }
}

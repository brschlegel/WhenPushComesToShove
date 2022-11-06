using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ObjectSpawned(Transform t);
public abstract class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToSpawn;
    [SerializeField]
    private Transform spawnedObjectParent;
    [SerializeField]
    private bool repeating;

    public event ObjectSpawned onObjectSpawned;
    public float spawnDelay;
    void Start()
    {
        
    }

    public abstract Vector2 GetSpawnLocation();

    public void SpawnObject()
    {
        Transform t = Instantiate(objectToSpawn, GetSpawnLocation(), Quaternion.identity, spawnedObjectParent).transform;
        onObjectSpawned?.Invoke(t);
    }

    private IEnumerator CoroutineSpawnWithDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnObject();
        if(repeating)
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
        SpawnObject();
    }

    public GameObject ObjectToSpawn
    {
        get {return objectToSpawn;}
        set {objectToSpawn = value;}
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            SpawnObject();
        }
    }
}

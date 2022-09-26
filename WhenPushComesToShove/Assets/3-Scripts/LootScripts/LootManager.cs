using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public LootData[] avaliableLoot;
    public List<LootData> droppedLoot;

    public float lootRadius = 5;
    public float timeBetweenSpawns = 5;

    // Start is called before the first frame update
    void Start()
    {
        droppedLoot = new List<LootData>();
        avaliableLoot = Resources.LoadAll<LootData>("Loot/");

        LevelManager.onNewRoom += ClearDroppedLoot;
        LevelManager.onEndGame += ClearDroppedLoot;

        //StartCoroutine(TestSpawnOverTime());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ClearDroppedLoot();
        }
    }

    /// <summary>
    /// Picks a random piece of loot from avaliable loot and spawns it
    /// </summary>
    /// <param name="position"></param>
    public void DropRandomLoot( Vector2 position )
    {
        int index = Random.Range(0, avaliableLoot.Length);
        GameObject lootObject = Instantiate(avaliableLoot[index].gameObject, position, Quaternion.identity);
        droppedLoot.Add(lootObject.GetComponent<LootData>());
    }

    /// <summary>
    /// Used to Debug Loot. Spawns a Piece of Loot Every 10 sec.
    /// </summary>
    /// <returns></returns>
    public IEnumerator TestSpawnOverTime()
    {
        yield return new WaitForSeconds(timeBetweenSpawns);
        DropRandomLoot(FindRandomPointInRadius());
        StartCoroutine(TestSpawnOverTime());
    }

    /// <summary>
    /// Destroys and clears any dropped loot in the current room
    /// </summary>
    public void ClearDroppedLoot()
    {
        for (int i = 0; i < droppedLoot.Count; i++)
        {
            Destroy(droppedLoot[i].gameObject);
        }

        droppedLoot.Clear();
    }

    private Vector3 FindRandomPointInRadius()
    {
        float x = Random.Range(transform.position.x - lootRadius, transform.position.x + lootRadius);
        float y = Random.Range(transform.position.y - lootRadius, transform.position.y + lootRadius);

        return new Vector3(x, y, 0);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public LootData[] avaliableLoot;
    public List<LootData> droppedLoot;

    // Start is called before the first frame update
    void Start()
    {
        droppedLoot = new List<LootData>();
        avaliableLoot = Resources.LoadAll<LootData>("Loot/");
        StartCoroutine(TestSpawnOverTime());
    }

    /// <summary>
    /// Picks a random piece of loot from avaliable loot and spawns it
    /// </summary>
    /// <param name="position"></param>
    public void DropRandomLoot( Vector2 position )
    {
        int index = Random.Range(0, avaliableLoot.Length);
        GameObject lootObject = Instantiate(avaliableLoot[index].gameObject, transform);
        droppedLoot.Add(avaliableLoot[index]);
    }

    /// <summary>
    /// Used to Debug Loot. Spawns a Piece of Loot Every 10 sec.
    /// </summary>
    /// <returns></returns>
    public IEnumerator TestSpawnOverTime()
    {
        yield return new WaitForSeconds(10);
        DropRandomLoot(Vector2.zero);
        StartCoroutine(TestSpawnOverTime());
    }

}

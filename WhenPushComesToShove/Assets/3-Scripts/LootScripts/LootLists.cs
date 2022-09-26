using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Loot List", menuName = "Loot")]
public class LootLists : ScriptableObject
{
    public LootData[] lootToSpawn;
}

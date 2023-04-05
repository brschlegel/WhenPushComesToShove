using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NintenSlime : BaseModifier
{
    [SerializeField]
    private GameObject slimePrefab;

    private GameObject spawnedSlime;
    // Start is called before the first frame update
    public override void Init()
    {
        spawnedSlime = Instantiate(slimePrefab);
    }

    public override void CleanUp()
    {
        Destroy(spawnedSlime);
    }
}

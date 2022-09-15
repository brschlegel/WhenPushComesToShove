using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathEndCondition : BaseEndCondition
{
    GameObject enemyPool;
    protected override void Start()
    {
        enemyPool = GameObject.FindGameObjectWithTag("EnemyPool");
        base.Start();
    }

    protected override void TestCondition()
    {
        //Prevents object reference errors
        if (enemyPool == null || levelProp.enemySpawnProps == null)
            return;

        //Won't progress until all spawn points are done spawning enemies
        foreach(EnemySpawnPoint spawnPoint in levelProp.enemySpawnProps)
        {
            if (spawnPoint.complete == false)
                return;
        }

        //Won't progess until all the enemies are dead
        if (enemyPool.transform.childCount > 0)
            return;

        base.TestCondition();
    }
}

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

        Debug.Log(levelProp.waveManager);

        if (!levelProp.waveManager.complete)
            return;

        base.TestCondition();
    }
}

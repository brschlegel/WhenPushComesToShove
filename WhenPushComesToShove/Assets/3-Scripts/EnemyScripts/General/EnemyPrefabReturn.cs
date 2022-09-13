using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrefabReturn : MonoBehaviour
{
    [HideInInspector]static Object[] allEnemies;
    //private void Awake()
    //{
    //    if(allEnemies == null)
    //        allEnemies = Resources.LoadAll("Enemy"); 
    //}
    static public GameObject ReturnEnemyPrefab(EnemyTypes type)
    {
        GameObject enemy = null;

        switch (type)
        {
            case EnemyTypes.Slimes:
                enemy = FindEnemy("Slime");
                break;
        }
        return enemy;
    }

    static GameObject FindEnemy(string name)
    {
        if (allEnemies == null)
            allEnemies = Resources.LoadAll("Enemy");

        foreach (Object obj in allEnemies)
        {
            if (obj.name == name)
                return (GameObject)obj;
        }
        return null;
    }
}

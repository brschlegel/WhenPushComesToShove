using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrefabReturn : MonoBehaviour
{
    public static EnemyPrefabReturn instance;
    [SerializeField] GameObject[] allEnemies;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }

    public GameObject ReturnEnemyPrefab(EnemyTypes type)
    {
        GameObject enemy = null;

        switch (type)
        {
            case EnemyTypes.Slimes:
                enemy = FindEnemy("Slime");
                break;

            case EnemyTypes.Aegis:
                enemy = FindEnemy("Aegis");
                break;
        }
        return enemy;
    }

    GameObject FindEnemy(string name)
    {
        foreach (GameObject obj in allEnemies)
        {
            if (obj.name == name)
                return obj;
        }
        return null;
    }
}

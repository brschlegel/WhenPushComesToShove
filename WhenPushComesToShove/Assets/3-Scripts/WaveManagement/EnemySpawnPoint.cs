using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] EnemyWaveSpawn.EnemyWaveStats[] enemyWaveStats;

    public int currentWave = 0;
    [HideInInspector] public bool waveComplete = false;
    public bool complete = false;

    List<GameObject> currentEnemies = new List<GameObject>();
    List<EnemyWaveSpawn.EnemyWaveStats> enemiesInWave = new List<EnemyWaveSpawn.EnemyWaveStats>();

    int enemyCount = 0;
    int waveEnemyMax = 0;

    GameObject enemyPool;

    public void Start()
    {
        enemyPool = GameObject.FindGameObjectWithTag("EnemyPool");
    }

    public void SpawnWave()
    {
        waveComplete = false;
        //Populate a list with the enemies of this wave       
        foreach(EnemyWaveSpawn.EnemyWaveStats stat in enemyWaveStats)
        {
            if(stat.waveNum == currentWave)
            {
                enemiesInWave.Add(stat);
                waveEnemyMax++;
            }
        }

        //Triggers that the there are no more waves in this spawn point
        if(enemiesInWave.Count <= 0)
        {
            complete = true;
            return;
        }

        //Begin spawning the enemies
        if (gameObject.activeInHierarchy)
        {
            foreach (EnemyWaveSpawn.EnemyWaveStats stat in enemiesInWave)
            {
                StartCoroutine(SpawnEnemy(EnemyPrefabReturn.ReturnEnemyPrefab(stat.enemy), stat.spawnDelay));
            }
        }

        //Test if the wave is complete
        StartCoroutine(TestWaveComplete());
        
    }

    IEnumerator SpawnEnemy(GameObject prefab, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
        obj.transform.parent = enemyPool.transform;
        enemyCount++;
        //currentEnemies.Add(obj);
    }

    IEnumerator TestWaveComplete()
    {
        yield return new WaitUntil(() => enemyCount >= waveEnemyMax);

        //Reset all values
        enemyCount = 0;
        waveEnemyMax = 0;
        enemiesInWave.Clear();

        waveComplete = true;
        currentWave++;
    }
}

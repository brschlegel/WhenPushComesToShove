using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] EnemyWaveSpawn.EnemyWaveStats[] enemyWaveStats;

    public int currentWave = 0;
    [HideInInspector] public bool waveComplete = false;
 
    
    private List<List<EnemyWaveSpawn.EnemyWaveStats>> waves;
 
    GameObject enemyPool;



    public void Init(int maxWaves)
    {
        enemyPool = GameObject.FindGameObjectWithTag("EnemyPool");
        waves = new List<List<EnemyWaveSpawn.EnemyWaveStats>>();
        for (int i = 0; i < maxWaves; i++)
        {
            waves.Add(new List<EnemyWaveSpawn.EnemyWaveStats>());
        }

        //Setting up all wave lists
        foreach(EnemyWaveSpawn.EnemyWaveStats e in enemyWaveStats)
        {
            waves[e.waveNum].Add(e);
        }
    }

    public IEnumerator SpawnWave(int waveNum)
    {
// ;       waveComplete = false;
//          foreach(EnemyWaveSpawn.EnemyWaveStats stat in waves[waveNum])
//         {
//             yield return new WaitForSeconds(stat.spawnDelay);
//             GameObject obj = Instantiate(EnemyPrefabReturn.instance.ReturnEnemyPrefab(stat.enemy), transform.position, Quaternion.identity);
//             obj.transform.parent = enemyPool.transform;
//         }

//         waveComplete = true;
        yield return new WaitForSeconds(1);
    }


}

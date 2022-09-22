using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemySpawnCondition {OnStart, InCamera};
public class WaveManager : MonoBehaviour
{
    [SerializeField] private EnemySpawnCondition enemySpawnCondition;

    public List<float> waveDelays;
    private int waveCount;
    [SerializeField]
    private int currentWave = -1;
    private List<EnemySpawnPoint> spawnPoints;

    private Transform enemyPool;
    private IEnumerator delayRoutine;
    //[HideInInspector]
    public bool complete;

    private void OnEnable()
    {
        if (spawnPoints == null)
        {
            currentWave = -1;
            complete = false;
            spawnPoints = new List<EnemySpawnPoint>(GetComponentsInChildren<EnemySpawnPoint>());
            enemyPool = GameObject.FindGameObjectWithTag("EnemyPool").transform;

            //Will only allow a spawn point 1 wave if it starts spawning in the camera
            if(enemySpawnCondition == EnemySpawnCondition.OnStart)
                waveCount = waveDelays.Count + 1;
            else if(enemySpawnCondition == EnemySpawnCondition.InCamera)
                    waveCount = 1;

            foreach (EnemySpawnPoint e in spawnPoints)
            {
                e.Init(waveCount);
            }
        }
        else
        {
            if(enemySpawnCondition == EnemySpawnCondition.OnStart)
                SpawnCurrentWave();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemySpawnCondition == EnemySpawnCondition.OnStart)
        {
            if (CheckAllWaveComplete())
            {
                if (currentWave < waveCount - 1)
                {
                    if (delayRoutine == null)
                    {
                        delayRoutine = DelaySpawnWave();
                        StartCoroutine(delayRoutine);
                    }
                    if (enemyPool.childCount == 0)
                    {
                        SpawnCurrentWave();
                        StopCoroutine(delayRoutine);
                    }
                }
                else if (enemyPool.childCount == 0)
                {
                    complete = true;
                }
            }
        }
        
    }

    /// <summary>
    /// Checks to see if the spawn points are done spawning their waves
    /// </summary>
    /// <returns></returns>
    private bool CheckAllWaveComplete()
    {
        
        foreach(EnemySpawnPoint e in spawnPoints)
        {
            if(!e.waveComplete)
                return false;
        }
        if (currentWave < waveCount - 1)
        {
            foreach (EnemySpawnPoint e in spawnPoints)
            {
                e.waveComplete = false;
            }
        }
     
        return true;
    }
    

    private IEnumerator DelaySpawnWave()
    {
        yield return new WaitForSeconds(waveDelays[currentWave]);
        SpawnCurrentWave();
        delayRoutine = null;
    }

    /// <summary>
    /// Will spawn the current wave on all enemy spawn points
    /// </summary>
    private void SpawnCurrentWave()
    {
        currentWave++;
        foreach (EnemySpawnPoint e in spawnPoints)
        {
            StartCoroutine(e.SpawnWave(currentWave));
        }
    }

    /// <summary>
    /// Tests if all of the enemies are dead in this room
    /// </summary>
    /// <returns></returns>
    public bool AllEnemiesDead()
    {
        return currentWave == waveCount - 1 && enemyPool.childCount == 0;
    }
}

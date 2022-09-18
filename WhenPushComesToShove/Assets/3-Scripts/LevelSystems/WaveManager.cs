using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private List<float> waveDelays;
    [SerializeField]
    private int currentWave = -1;
    private List<EnemySpawnPoint> spawnPoints;

    private Transform enemyPool;
    private IEnumerator delayRoutine;
    //[HideInInspector]
    public bool complete;
    private bool lastWaveComplete;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    private void OnEnable()
    {
        if (spawnPoints == null)
        {
            currentWave = -1;
            complete = false;
            spawnPoints = new List<EnemySpawnPoint>(GetComponentsInChildren<EnemySpawnPoint>());
            enemyPool = GameObject.FindGameObjectWithTag("EnemyPool").transform;
            foreach (EnemySpawnPoint e in spawnPoints)
            {
                e.Init(waveDelays.Count + 1);
            }
        }
        else
        {
            SpawnAllWaves();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckAllWaveComplete())
        {
            if (currentWave < waveDelays.Count)
            {
                if (delayRoutine == null)
                {
                    delayRoutine = DelaySpawnWave();
                    StartCoroutine(delayRoutine);
                }
                if (enemyPool.childCount == 0)
                {
                    SpawnAllWaves();
                    StopCoroutine(delayRoutine);
                }
            }
            else if(enemyPool.childCount == 0)
            {
                complete = true;
            }
               

        }



       
    }

    private bool CheckAllWaveComplete()
    {
        
        foreach(EnemySpawnPoint e in spawnPoints)
        {
            if(!e.waveComplete)
                return false;
        }
        if (currentWave < waveDelays.Count)
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
        SpawnAllWaves();
        delayRoutine = null;
    }

    private void SpawnAllWaves()
    {
        currentWave++;
        foreach (EnemySpawnPoint e in spawnPoints)
        {
            StartCoroutine(e.SpawnWave(currentWave));
        }
    }

    public bool AllEnemiesDead()
    {
        return currentWave == waveDelays.Count && enemyPool.childCount == 0;
    }
}

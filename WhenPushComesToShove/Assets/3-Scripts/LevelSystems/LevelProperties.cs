using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Hazards { None, Spikes, Spring, Fire };
public enum EnemyTypes { General, Slimes};

public enum LevelType { Dungeon, Arena};

public class LevelProperties : MonoBehaviour
{
    //Properties needed for the level editor
    public HazardDifficulty.HazardStats[] hazards;
    public EnemyDifficulty.EnemyLevelStats[] enemyStats;
    public GameObject[] playerSpawns;
    public GameObject[] enemySpawns;
    public LevelType levelType;

    [HideInInspector] public List<EnemySpawnPoint> enemySpawnProps = new List<EnemySpawnPoint>();

    private void OnEnable()
    {
        foreach (GameObject obj in enemySpawns)
        {
            EnemySpawnPoint currentPoint = obj.GetComponent<EnemySpawnPoint>();
            if (currentPoint)
                currentPoint.currentWave = 0;

            enemySpawnProps.Add(currentPoint);
            
        }
    }

    private void OnDisable()
    {
        enemySpawnProps.Clear();
    }

    private void Update()
    {
        if(enemySpawnProps.Count > 0)
        {
            //Will ensure that the wave only changes if all of the spawns have completed the current wave
            foreach(EnemySpawnPoint spawn in enemySpawnProps)
            {
                if (spawn.waveComplete == false)
                    return;
            }

            foreach (EnemySpawnPoint spawn in enemySpawnProps)
            {
                spawn.SpawnWave();
            }


        }
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Hazards { None, Spikes, Spring, Fire };
public enum EnemyTypes { General, Slimes, Aegis};

public enum LevelType { Dungeon, Arena};

public class LevelProperties : MonoBehaviour
{
    //Properties needed for the level editor
    public HazardDifficulty.HazardStats[] hazards;
    public EnemyDifficulty.EnemyLevelStats[] enemyStats;
    public GameObject[] playerSpawns;
    public GameObject[] enemySpawns;
    public LevelType levelType;
    public WaveManager waveManager;

    [HideInInspector] public List<EnemySpawnPoint> enemySpawnProps = new List<EnemySpawnPoint>();

    private void OnEnable()
    {
       
    }

    private void OnDisable()
    {
        
    }
    
}

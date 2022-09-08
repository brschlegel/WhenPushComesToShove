using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Hazards { None, Spikes, Spring, Fire };
public enum EnemyTypes { General, Slimes};

public class LevelProperties : MonoBehaviour
{
    public HazardDifficulty.HazardStats[] hazards;
    public EnemyDifficulty.EnemyLevelStats[] enemyStats;
    public GameObject[] playerSpawns;
    public GameObject[] enemySpawns;
}

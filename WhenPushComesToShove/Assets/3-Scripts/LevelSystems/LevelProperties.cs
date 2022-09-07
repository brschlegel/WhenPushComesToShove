using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Hazards { None, Spikes, Spring, Fire };

public class LevelProperties : MonoBehaviour
{
    public HazardDifficulty.HazardStats[] hazards;
    public GameObject[] playerSpawns;
    public GameObject[] enemySpawns;
}

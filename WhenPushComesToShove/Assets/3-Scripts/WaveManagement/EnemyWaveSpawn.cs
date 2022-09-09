using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawn : MonoBehaviour
{
    public struct EnemyWaveStats
    {
        public EnemyTypes enemy;
        public float spawnDelay;
        public int waveNum;
    }
}

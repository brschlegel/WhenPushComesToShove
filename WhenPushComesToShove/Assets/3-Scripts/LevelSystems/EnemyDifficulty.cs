using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDifficulty : MonoBehaviour
{
    [Serializable]
    public struct EnemyLevelStats
    {
        public EnemyTypes enemy;
        public int level;
    }
}

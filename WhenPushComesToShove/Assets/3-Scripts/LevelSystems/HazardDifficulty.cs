using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardDifficulty : MonoBehaviour
{
    [Serializable]
    public struct HazardStats
    {
        public Hazards hazard;
        public int level;
    }
}

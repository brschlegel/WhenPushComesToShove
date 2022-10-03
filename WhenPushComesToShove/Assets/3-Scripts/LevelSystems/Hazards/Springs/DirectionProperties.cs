using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DirectionProperties : MonoBehaviour
{
    [Serializable]
    public struct Direction
    {
        public bool enabled;
        public string name;
        public Vector2 direction;
    }
}

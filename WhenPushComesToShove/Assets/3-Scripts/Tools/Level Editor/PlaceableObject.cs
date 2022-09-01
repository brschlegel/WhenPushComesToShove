using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    [Serializable]
    public struct ObjectStats
    {
        public string objectName;
        public Sprite objectSprite;
        public GameObject objectPrefab;
    }
   
}

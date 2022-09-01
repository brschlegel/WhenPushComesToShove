using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField] HazardDifficulty.HazardStats[] hazardLevels;
    Object[] levels;

    // Start is called before the first frame update
    void Start()
    {
        levels = Resources.LoadAll("Levels");

        Instantiate((GameObject)levels[0], new Vector3(0,0,0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

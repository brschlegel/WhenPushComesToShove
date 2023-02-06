using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Hazards { None, Spikes, Spring, Fire };
public enum EnemyTypes { General, Slimes, Aegis};

public enum LevelType { Lobby, Dungeon, Arena, TwoTwo, ThreeOne, Modifier};

public class LevelProperties : MonoBehaviour
{
    //Properties needed for the level editor
    public GameObject[] playerSpawns;
    public GameObject[] enemySpawns;
    public LevelType levelType;
    public Minigame game;

    [HideInInspector] public bool teamLevel;


    private void OnEnable()
    {
        if (levelType == LevelType.TwoTwo || levelType == LevelType.ThreeOne)
            teamLevel = true;
        else
            teamLevel = false;
    }

    private void OnDisable()
    {
        
    }
    
}

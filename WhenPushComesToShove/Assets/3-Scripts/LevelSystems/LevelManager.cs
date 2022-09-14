using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    int currentRoomIndex = 0;
    PathGenerator pathGen;
    List<GameObject> path;
    [Tooltip("Debug Variable. Will cause the path to cycle to the beginning.")]
    [SerializeField] bool repeatPath;
    public static Action onNewRoom;
    EnemySpawnPoint[] enemySpawns;

    private void OnEnable()
    {
        onNewRoom += ShowRoom;
    }

    private void OnDisable()
    {
        onNewRoom -= ShowRoom;
    }

    private void Awake()
    {
        pathGen = GetComponent<PathGenerator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Temp code to test if the room transitions work
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject enemyPool = GameObject.FindGameObjectWithTag("EnemyPool");
            for(int i = enemyPool.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(enemyPool.transform.GetChild(i).gameObject);
            }

            onNewRoom();
        }
    }

    //Will ensure that only the current room on the path will show up
    void ShowRoom()
    {
        if (path == null)
            path = pathGen.path;

        if (currentRoomIndex >= path.Count && repeatPath == false)
            return;
        else if (currentRoomIndex >= path.Count && repeatPath == true)
        {
            currentRoomIndex = 0;
        }
            

        
        //Hide the previous room
        for (int i = 0; i < path.Count; i++)
        {
            if (path[i].activeInHierarchy)
                path[i].SetActive(false);
        }

        //Show the new room
        GameObject room = path[currentRoomIndex];
        room.SetActive(true);

        //Grab the enemy spawn points
        LevelProperties levelProp = room.GetComponent<LevelProperties>();
        
        if(levelProp.enemySpawnProps.Count > 0)
        {
            foreach (EnemySpawnPoint spawn in levelProp.enemySpawnProps)
            {
                spawn.SpawnWave();
            }
        }
        

        currentRoomIndex++;

        SetPlayerSpawns(room.GetComponent<LevelProperties>());
    }

    /// <summary>
    /// Sets all of the players to their respective spawn points
    /// </summary>
    /// <param name="levelProps">Properties of the current room</param>
    void SetPlayerSpawns(LevelProperties levelProps)
    {
        InitLevel init = PlayerConfigManager.Instance.levelInitRef;

        //Convert the gameobjects into transfomrs
        Transform[] levelPlayerSpawn = new Transform[init.playerSpawns.Length];
        for (int i = 0; i < levelPlayerSpawn.Length; i++)
        {
            levelPlayerSpawn[i] = levelProps.playerSpawns[i].transform;
        }

        //Set the players spawn positions
        init.playerSpawns = levelPlayerSpawn;

        //Debug.Log(init.lockPlayerSpawn);
        if(init.lockPlayerSpawn)
            init.SpawnPlayersInLevel();
    }
}

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
    private DamageEnabler damageEnabler;
    public static Action onNewRoom;
    public static Action onModifierRoom;
    public static Action onEndGame;
    private bool endRoomSpawned = false;

    private void OnEnable()
    {
        onNewRoom += ShowRoom;
        onEndGame += ResetPath;
    }

    private void OnDisable()
    {
        onNewRoom -= ShowRoom;
        onEndGame -= ResetPath;
    }

    private void Awake()
    {
   
        pathGen = GetComponent<PathGenerator>();
        damageEnabler = GetComponent<DamageEnabler>();
        
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
            onNewRoom();
        }

        //Temp code to reset the game fully
        if (Input.GetKeyDown(KeyCode.Q))
        {
            onEndGame();
        }
    }

    //Will ensure that only the current room on the path will show up
    void ShowRoom()
    {

        //Spawn the end room if at the end of the path or if there's no avaiable minigames
        if (currentRoomIndex > pathGen.numOfRooms || pathGen.availableLevels.Count <= 0)
        {
            SpawnEndRoom();
            return;
        }

        damageEnabler.EnableDamage(currentRoomIndex > 0);

        //Clears previous level
        if(pathGen.transform.childCount > 0)
        {
            Destroy(pathGen.transform.GetChild(0).gameObject);
        }

        //Show the new room
        GameObject room = null;

        //Spawns the lobby
        if (currentRoomIndex == 0)
        {
            room = pathGen.lobby;
        }
        //Spawns a minigame
        else if(currentRoomIndex > 0 && currentRoomIndex % 2 == 1)
        {
            room = pathGen.AssignLevel().gameObject;
        }
        //Spawns a modifier room
        else if(currentRoomIndex > 0 && currentRoomIndex % 2 == 0)
        {
            room = pathGen.modifierRoom;
        }

        GameObject newRoom = pathGen.SpawnRoom(room);

        //Grab the properties for this level
        LevelProperties levelProp = newRoom.GetComponent<LevelProperties>();

        currentRoomIndex++;

        GameState.currentRoomType = levelProp.levelType;
        SetPlayerSpawns(levelProp);   
     
        if(levelProp.transform.GetChild(3).TryGetComponent<MinigameLogic>(out MinigameLogic logic))
        {
            logic.Init();
            GameState.ModifierManager.InitMinigame(levelProp.transform);
        }

        //Update Logging
        LoggingInfo.instance.currentRoomName = newRoom.name;
        LoggingInfo.instance.numOfRoomsTraveled++;
    }

    public void ResetPath()
    {
        //Temp code - Will just put everyone back into the lobby

      

        currentRoomIndex = 0;
        endRoomSpawned = false;
        //currentRoomIndex = path.Count;
        //repeatPath = true;
        //ShowRoom();     
        //repeatPath = false;

        //Resets any players who died
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in players)
        {
            obj.GetComponentInChildren<Health>().dead = false;
        }

        pathGen.ResetPath();
        //Should remake the path
    }

    public void SpawnEndRoom()
    {

        if (endRoomSpawned)
        {
            ResetPath();
        }
        else
        {
            //Clears previous level
            if (pathGen.transform.childCount > 0)
            {
                Destroy(pathGen.transform.GetChild(0).gameObject);
            }

            //Reset Players Health
            foreach (Transform p in GameState.players)
            {
                p.GetComponentInChildren<PlayerHealth>().ResetHealth();
            }

            GameObject room = pathGen.victoryRoom;

            GameObject newRoom = pathGen.SpawnRoom(room);

            //Grab the properties for this level
            LevelProperties levelProp = newRoom.GetComponent<LevelProperties>();

            GameState.currentRoomType = levelProp.levelType;
            SetPlayerSpawns(levelProp);

            endRoomSpawned = true;
        }
    }

    /// <summary>
    /// Sets all of the players to their respective spawn points
    /// </summary>
    /// <param name="levelProps">Properties of the current room</param>
    public void SetPlayerSpawns(LevelProperties levelProps)
    {
        //Convert the gameobjects into transforms
        InitLevel init = PlayerConfigManager.Instance.levelInitRef;       
        Transform[] levelPlayerSpawn = new Transform[init.playerSpawns.Length];

        for (int i = 0; i < levelPlayerSpawn.Length; i++)
        {
            levelPlayerSpawn[i] = levelProps.playerSpawns[i].transform;
        }

        //Set the players spawn positions
        init.playerSpawns = levelPlayerSpawn;

        //Debug.Log(init.lockPlayerSpawn);
        if(init.lockPlayerSpawn)
            init.SpawnPlayersInLevel(levelProps);
    }
}

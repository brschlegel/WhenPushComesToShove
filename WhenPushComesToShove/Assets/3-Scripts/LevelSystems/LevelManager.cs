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
    [SerializeField]
    private ModifierManager modifierManager;

    private void OnEnable()
    {
        onNewRoom += ShowRoom;
        onModifierRoom += ShowModifierRoom;
        onEndGame += ResetPath;
    }

    private void OnDisable()
    {
        onNewRoom -= ShowRoom;
        onModifierRoom -= ShowModifierRoom;
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
            //Will transition to a new game
            if (GameState.currentRoomType == LevelType.Modifier)
                onNewRoom();
            //Will transition to a modifier room
            else if (GameState.currentRoomType != LevelType.Lobby && GameState.currentRoomType != LevelType.Modifier)
                onModifierRoom();
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
        //Resets to the beginnig if there's no more available games or if its reached the max number of rooms
        //if(currentRoomIndex > pathGen.numOfDungeonRooms || pathGen.availableLevels.Count <= 0)
        //{
        //    ResetPath();
        //    return;
        //}

        damageEnabler.EnableDamage(currentRoomIndex > 0);

        //Clears previous level
        if(pathGen.transform.childCount > 0)
        {
            Destroy(pathGen.transform.GetChild(0).gameObject);
        }

        //Show the new room
        GameObject room = null;

        if (currentRoomIndex == 0)
        {
            room = pathGen.lobby;
        }
        else
        {
            room = pathGen.AssignLevel().gameObject;
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

        }

        //Update Logging
        LoggingInfo.instance.currentRoomName = newRoom.name;
        LoggingInfo.instance.numOfRoomsTraveled++;
    }

    void ShowModifierRoom()
    {
        // Resets to the beginnig if there's no more available games or if its reached the max number of rooms
        if (currentRoomIndex > pathGen.numOfDungeonRooms || pathGen.availableLevels.Count <= 0)
        {
            ResetPath();
            return;
        }

        //Clears previous level
        if (pathGen.transform.childCount > 0)
        {
            Destroy(pathGen.transform.GetChild(0).gameObject);
        }

        GameObject room = pathGen.modifierRoom;

        GameObject newRoom = pathGen.SpawnRoom(room);

        //Grab the properties for this level
        LevelProperties levelProp = newRoom.GetComponent<LevelProperties>();
        if (levelProp.transform.GetChild(3).TryGetComponent<MinigameLogic>(out MinigameLogic logic))
        {
            logic.Init();

        }


        GameState.currentRoomType = levelProp.levelType;
        SetPlayerSpawns(levelProp);
    }
    public void ResetPath()
    {
        //Temp code - Will just put everyone back into the lobby

      

        currentRoomIndex = 0;
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

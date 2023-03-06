 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    int currentRoomIndex = 0;
    PathGenerator pathGen;
    [Tooltip("Debug Variable. Will cause the path to cycle to the beginning.")]
    [SerializeField] bool repeatPath;
    private DamageEnabler damageEnabler;
    public static Action onNewRoom;
    public static Action onModifierRoom;
    public static Action onEndGame;
    private bool endRoomSpawned = false;
    private MinigameLogic currentRoom;

    private void OnEnable()
    {
        onNewRoom += ShowRoom;
        onEndGame += ResetGame;
    }

    private void OnDisable()
    {
        onNewRoom -= ShowRoom;
        onEndGame -= ResetGame;
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
        ////Temp code to test if the room transitions work
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    if(GameState.currentRoomType == LevelType.Modifier)
        //    {
        //        ModifierSelectionLogic modifierRoom = currentRoom.GetComponent<ModifierSelectionLogic>();
                
        //        modifierRoom.OnSelectionFinished(0);
        //        modifierRoom.StopAllCoroutines();
        //        modifierRoom.DebugCleanUp();
        //    }
        //    else
        //    {
        //        //Ensures that a new minigame will be set
        //        List<Minigame> allMinigames = new List<Minigame>();
        //        allMinigames.Add(Minigame.All);
        //        pathGen.PopulateAvailableLevels(allMinigames);

        //        currentRoom.DebugCleanUp();
        //    }

            


        //}

        ////Temp code to reset the game fully
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    onEndGame();
        //}

        //Hotkeys to add games to the path

        //Dodgeball
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {            
            pathGen.path.Add(pathGen.allLevels[0].gameObject);
            
        }

        //Hot Potato
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            pathGen.path.Add(pathGen.allLevels[1].gameObject);
        }

        //Soccer
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            pathGen.path.Add(pathGen.allLevels[2].gameObject);
        }

        //Sumo
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            pathGen.path.Add(pathGen.allLevels[3].gameObject);
        }
       
        //Tag
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            pathGen.path.Add(pathGen.allLevels[4].gameObject);
        }

        //Volley Bomb
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            pathGen.path.Add(pathGen.allLevels[5].gameObject);
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

        //Is this valid anymore?
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
            currentRoom = logic;
            Debug.Log(currentRoom.gameObject.name);
            logic.Init();
            GameState.ModifierManager.InitMinigame(levelProp.transform);
        }

        //Update Logging
        LoggingInfo.instance.currentRoomName = newRoom.name;
        LoggingInfo.instance.numOfRoomsTraveled++;
    }

    public void ResetGame()
    {
        //Temp code - Will just put everyone back into the lobby

        GameState.ModifierManager.RemoveAllModifiers();

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
            obj.GetComponentInChildren<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }

        pathGen.ResetPath();
        //Should remake the path
    }

    public void SpawnEndRoom()
    {

        if (endRoomSpawned)
        {
            ResetGame();
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

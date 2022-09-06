using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    int currentRoomIndex = 0;
    PathGenerator pathGen;
    List<GameObject> path;
    public static Action onNewRoom;

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
        if (Input.anyKeyDown)
        {
            onNewRoom();
        }
    }

    //Will ensure that only the current room on the path will show up
    void ShowRoom()
    {
        if (path == null)
            path = pathGen.path;

        if (currentRoomIndex >= path.Count)
            return;

        //Hide the previous room
        for (int i = 0; i < path.Count; i++)
        {
            if (path[i].activeInHierarchy)
                path[i].SetActive(false);
        }

        //Show the new room
        GameObject room = path[currentRoomIndex];
        room.SetActive(true);

        currentRoomIndex++;

        SetPlayerSpawns(room.GetComponent<LevelProperties>());
    }

    /// <summary>
    /// Sets all of the players to their respective spawn points
    /// </summary>
    /// <param name="levelProps">Properties of the current room</param>
    void SetPlayerSpawns(LevelProperties levelProps)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(("Player"));
        
        for(int i = 0; i < players.Length; i++)
        {
            players[i].transform.position = levelProps.playerSpawns[i].transform.position;
        }
    }
}

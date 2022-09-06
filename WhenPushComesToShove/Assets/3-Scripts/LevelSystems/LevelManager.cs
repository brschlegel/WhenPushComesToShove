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

    // Start is called before the first frame update
    void Start()
    {
        pathGen = GetComponent<PathGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowRoom()
    {
        
        if (path == null)
            path = pathGen.path;

        //Hide the previous room
        for(int i = 0; i < path.Count; i++)
        {
            if (path[i].activeInHierarchy)
                path[i].SetActive(false);
        }

        //Show the new room
        GameObject room = path[currentRoomIndex];
        Debug.Log(room.name);
        room.SetActive(true);
        Debug.Log(room.activeInHierarchy);

        currentRoomIndex++;
    }
}

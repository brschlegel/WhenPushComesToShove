using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inits the level based on player data
public class InitLevel : MonoBehaviour
{
    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private bool initOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        if (initOnStart)
        {
            
        }
        else
        {
            //Pass in this reference so that players can be spwaned when they join
            PlayerConfigManager.Instance.levelInitRef = this;
        }

    }

    /// <summary>
    /// Used if players are not spawned on start
    /// </summary>
    public void SpawnPlayer( int index )
    {
        PlayerConfiguration[] playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        GameObject player = Instantiate(playerPrefab, playerSpawns[index].position, playerSpawns[index].rotation, gameObject.transform);
        player.GetComponentInChildren<PlayerInputHandler>().InitializePlayer(playerConfigs[index]);
        playerConfigs[index].PlayerObject = player;
    }

    public void SpawnPlayersInLevel()
    {
        //Prevent players from being spawned separately
        PlayerConfigManager.Instance.levelInitRef = null;

        //Spawn the players in their set locations for the level.
        PlayerConfiguration[] playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            //GameObject player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            //player.GetComponentInChildren<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);

            playerConfigs[i].PlayerObject.transform.position = playerSpawns[i].transform.position;
        }
    }

}

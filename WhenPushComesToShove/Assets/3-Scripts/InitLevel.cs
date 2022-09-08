using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inits the level based on player data
public class InitLevel : MonoBehaviour
{
    public Transform[] playerSpawns;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private bool initOnStart = true;

    [HideInInspector] public bool lockPlayerSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerConfigManager.Instance.levelInitRef == null)
        {
            PlayerConfigManager.Instance.levelInitRef = this;
        }

    }

    /// <summary>
    /// Used if players are not spawned on start
    /// </summary>
    public void SpawnPlayer( int index )
    {
        if (lockPlayerSpawn)
        {
            return;
        }

        PlayerConfiguration[] playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        GameObject player = Instantiate(playerPrefab, playerSpawns[index].position, playerSpawns[index].rotation, gameObject.transform);
        GameState.players.Add(player.transform);
        player.GetComponentInChildren<PlayerInputHandler>().InitializePlayer(playerConfigs[index]);
        playerConfigs[index].PlayerObject = player;
    }

    public void SpawnPlayersInLevel()
    {

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

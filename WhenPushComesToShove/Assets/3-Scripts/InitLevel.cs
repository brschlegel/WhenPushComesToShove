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

    public GameObject[] playerUI= new GameObject[4];
    public Color[] playerHitboxColors = new Color[4];

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerConfigManager.Instance.levelInitRef == null)
        {
            PlayerConfigManager.Instance.levelInitRef = this;
        }

        LevelManager.onEndGame += UnlockPlayerSpawn;

    }

    /// <summary>
    /// Used if players are not spawned on start
    /// </summary>
    public void SpawnPlayer( int index )
    {
        PlayerConfiguration[] playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        GameObject player = Instantiate(playerPrefab, playerSpawns[index].position, playerSpawns[index].rotation, gameObject.transform);
        GameState.players.Add(player.transform);
        player.GetComponentInChildren<PlayerInputHandler>().InitializePlayer(playerConfigs[index]);

        //Assign and enable UI
        GameObject UI = player.GetComponentInChildren<PlayerInventory>().UIRef = playerUI[index];
        UI.SetActive(true);

        //Assign Hitbox Colors
        Hitbox[] hitboxes = player.GetComponentsInChildren<Hitbox>();

        foreach (Hitbox h in hitboxes)
        {
            h.gameObject.GetComponent<SpriteRenderer>().color = playerHitboxColors[index];
        }

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
            if (playerConfigs[i].IsDead)
            {
                playerConfigs[i].PlayerObject.GetComponentInChildren<PlayerHealth>().ResetHealth();
                playerConfigs[i].IsDead = false;
            }

            playerConfigs[i].PlayerObject.transform.position = playerSpawns[i].transform.position;
        }
    }

    /// <summary>
    /// Helper Function to Lock Player Spawn 
    /// </summary>
    public void LockPlayerSpawn()
    {
        lockPlayerSpawn = true;
    }

    /// <summary>
    /// Helper Function to Unlock Player Spawn If Possible
    /// </summary>
    public void UnlockPlayerSpawn()
    {
        PlayerConfiguration[] playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();

        if (playerConfigs.Length < PlayerConfigManager.Instance.GetMaxPlayers())
        {
            lockPlayerSpawn = false;
        }
    }

}
